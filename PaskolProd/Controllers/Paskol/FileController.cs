using Infrastructure.Configuration;
using Microsoft.AspNet.Identity;
using Model;
using Model.Users;
using PaskolProd.Authentication;
using Services;
using Services.Messaging.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace PaskolProd.Controllers.Paskol
{
    public class FileController : Controller
    {
        private const string USER_CONV = "User-";
        private const string ARTIST_AGREEMENT_FN = "ArtistAgreement_V0.pdf";
        private FileSystemService service { get; set; }
        private PurchaseService purchService { get; set; }

        public FileController()
        {
            service = ServiceLocator.GetService<FileSystemService>();
            purchService = ServiceLocator.GetService<PurchaseService>();
        }

        private void DownloadRange (string fullpath)
        {
            //http://blogs.visigo.com/chriscoulson/easy-handling-of-http-range-requests-in-asp-net/
            long size, start, end, length, fp = 0;
            using (StreamReader reader = new StreamReader(fullpath))
            {
                size = reader.BaseStream.Length;
                start = 0;
                end = size - 1;
                length = size;
                // Now that we've gotten so far without errors we send the accept range header
                /* At the moment we only support single ranges.
                 * Multiple ranges requires some more work to ensure it works correctly
                 * and comply with the spesifications: http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2
                 *
                 * Multirange support annouces itself with:
                 * header('Accept-Ranges: bytes');
                 *
                 * Multirange content must be sent with multipart/byteranges mediatype,
                 * (mediatype = mimetype)
                 * as well as a boundry header to indicate the various chunks of data.
                 */

                Response.AddHeader("Accept-Ranges", "0-" + size);
                // header('Accept-Ranges: bytes');
                // multipart/byteranges
                // http://www.w3.org/Protocols/rfc2616/rfc2616-sec19.html#sec19.2

                if (!String.IsNullOrEmpty(Request.ServerVariables["HTTP_RANGE"]))
                {
                    long anotherStart = start;
                    long anotherEnd = end;
                    string[] arr_split = Request.ServerVariables["HTTP_RANGE"].Split(new char[] { Convert.ToChar("=") });
                    string range = arr_split[1];

                    // Make sure the client hasn't sent us a multibyte range
                    if (range.IndexOf(",") > -1)
                    {
                        // (?) Shoud this be issued here, or should the first
                        // range be used? Or should the header be ignored and
                        // we output the whole content?
                        Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");

                    }

                    // If the range starts with an '-' we start from the beginning
                    // If not, we forward the file pointer
                    // And make sure to get the end byte if spesified
                    if (range.StartsWith("-"))
                    {
                        // The n-number of the last bytes is requested
                        anotherStart = size - Convert.ToInt64(range.Substring(1));
                    }
                    else
                    {
                        arr_split = range.Split(new char[] { Convert.ToChar("-") });
                        anotherStart = Convert.ToInt64(arr_split[0]);
                        long temp = 0;
                        anotherEnd = (arr_split.Length > 1 && Int64.TryParse(arr_split[1].ToString(), out temp)) ? Convert.ToInt64(arr_split[1]) : size;
                    }
                    /* Check the range and make sure it's treated according to the specs.
                     * http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
                     */
                    // End bytes can not be larger than $end.
                    anotherEnd = (anotherEnd > end) ? end : anotherEnd;
                    // Validate the requested range and return an error if it's not correct.
                    if (anotherStart > anotherEnd || anotherStart > size - 1 || anotherEnd >= size)
                    {

                        Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
                        throw new HttpException(416, "Requested Range Not Satisfiable");
                    }
                    start = anotherStart;
                    end = anotherEnd;

                    length = end - start + 1; // Calculate new content length
                    fp = reader.BaseStream.Seek(start, SeekOrigin.Begin);
                    Response.StatusCode = 206;
                }
            }
            // Notify the client the byte range we'll be outputting
            Response.AddHeader("Content-Range", "bytes " + start + "-" + end + "/" + size);
            Response.AddHeader("Content-Length", length.ToString());
            if (length > (1024 *1024*5))
            {
                length = (1024 * 1024 * 5);
            }
            Response.WriteFile(fullpath, fp, length);
            Response.End();

        }
        public void GetRange(Model.File file)
        {
            var res = service.GetFilePath(file);

            if (res.Success)
            {
                try
                {
                    DownloadRange(res.Entity);
                }
                catch (HttpException e)
                {
                    // The user cancel the request' it's OK
                }
                catch (Exception e)
                {
                    throw new HttpException(500, e.ToString());
                }
            }
            else
            {
                throw new HttpException(404, res.Message);
            }
        }

        public ActionResult PreviewPurchaseAgreement()
        {
            return File(Server.MapPath("/Content/PurchaseAgreement.pdf"), "application/pdf");
        }

        [Authorize(Roles = "Customer")]
        public ActionResult PurchaseAgreement(int musicID, int cost, string reference, string permission)
        {
            var user = User.GetUser();
            var res = purchService.GetPurchaseHTML(musicID, user, cost, reference, permission);
            if (res.Success)
            {
                return File(Encoding.UTF8.GetBytes(res.Entity), "text/html");
            }
            else
            {
                return Json("אירעה שגיאה בהבאת ההסכם");
            }
        }

        public ActionResult PersonalArtistAgreement(string UserId)
        {
            // only if this is your user
            if (User.Identity.GetUserId() == UserId)
            {
                string route = Path.Combine(WebConf.FSBaseRoute, "PDF", USER_CONV + UserId, ARTIST_AGREEMENT_FN);
                return GetByRoute(route, ARTIST_AGREEMENT_FN);
            }
            return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        public ActionResult GetFile(Model.File file, string FileName)
        {
            var res = service.GetFile(file);
            
            if (res.Success)
            {
                if (string.IsNullOrEmpty(FileName))
                {
                    return File(res.Entity.Content, res.Entity.ContentType);
                }

                return File(res.Entity.Content, res.Entity.ContentType,FileName + file.suffix);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }

        public ActionResult GetTermOfUse()
        {
            return GetByRoute(Server.MapPath("~/App_Data/TermOfUse.pdf"), "TermOfUse.pdf");
        }

        public ActionResult GetArtistContract()
        {
            return GetByRoute(Server.MapPath("~/App_Data/ArtistAgreement.pdf"), "ArtistAgreement.pdf");
        }

        private ActionResult GetByRoute(string path, string FileName)
        {
            var res = service.Get(path);

            if (res.Success)
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + FileName);
                return File(res.Entity.Content, res.Entity.ContentType);
            }

            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        }
    }
}