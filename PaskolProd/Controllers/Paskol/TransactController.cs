using System.Web.Mvc;
using Model.Filters;
using Model.Models;
using PaskolProd.Models;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Net;
using System;
using Model.Controllers.Helper;
using Newtonsoft.Json.Linq;
using Services;
using Model.Purchase;
using PaskolProd.Authentication;
using System.Collections.Generic;
using System.Linq;

namespace PaskolProd.Paskol.Controllers
{
    [Authorize]
    public class TransactController : Controller
    {
        private PurchaseService _PurchaseSrv;
        private PermissionCategoryService _permCatSrv;

        public TransactController()
        {
            _PurchaseSrv = ServiceLocator.GetService<PurchaseService>();
            _permCatSrv = ServiceLocator.GetService<PermissionCategoryService>();
        }

        private JsonTransactionResponse _GetUrl(PurchaseDetails pd)
        {
            // Fill in the JSON parameters (use the corresponding documents to determine the parameters needed for your unique payment page).
            var jsonTransactionRequest = new JsonTransactionDetails()
            {
                terminal = "0962210",
                user = "testpelecard1",
                password = "Q3EJB8Ah",
                GoodURL = CommonHelper.GetBaseUrl() + Url.Action("ResFromPlecard"),
                ErrorURL = CommonHelper.GetBaseUrl() + Url.Action("ResFromPlecard"),
                FeedbackDataTransferMethod = "POST",
                CssURL = CommonHelper.GetBaseUrl() + "/TemplatesContent/bootstrap/css/customPele.css",
                UserKey = Guid.NewGuid().ToString(),
                Total = ((int)(pd.Cost * 100)).ToString(),
                UserData = new UserData()
                {
                    UserData1 = string.Format("שם לקוח:{0}",pd.Customer.UserName),
                    UserData2 = string.Format("מזהה לקוח:{0}",pd.Customer.Id),
                    UserData3 = string.Format("שם יצירה:{0}", pd.Music.HebrewName),
                    UserData4 = string.Format("שימוש:{0}",string.Join(",", pd.PermissionValueNames.ToArray())),
                    UserData5 = pd.Music.HebrewName,
                    UserData6 = string.Join(",", pd.PermissionValueNames.ToArray()),
                    UserData7 = pd.Music.ID.ToString(),
                    UserData8 = pd.Permission.ID.ToString()
                }
            };

            return PostJson<JsonTransactionDetails, JsonTransactionResponse>(jsonTransactionRequest,
               "https://gateway20.pelecard.biz/PaymentGW/Init");
            }
        
        [HttpPost]
        [AllowAnonymous]
        [AJAXValidateAntiForgeryToken]
        public JsonResult GetUrl(IEnumerable<Guid> valueIds, int musicId, Guid categoryId, double cost)
        {
            var costRes = _permCatSrv.CheckPermissionCost(valueIds, musicId, categoryId,true);
            var isValid = costRes.Success && cost == costRes.Entity.Cost;

            if (isValid)
            {
                var purchaseDetails = new PurchaseDetails();
                purchaseDetails = costRes.Entity;
                purchaseDetails.Customer = User.GetUser();
                return Json(_GetUrl(purchaseDetails));
            }
            return null;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResFromPlecard(FormCollection collection)
        {
            const string SUCCESS_CODE = "000";
            bool Success;
            string url = "";
            string errMsg = "";

            // Get transact details
            var res = PostJson<GetTransactReq, GetTransactRes>(new GetTransactReq()
            {
                terminal = "0962210",
                user = "testpelecard1",
                password = "Q3EJB8Ah",
                TransactionId = collection["PelecardTransactionId"]
            },
            "https://gateway20.pelecard.biz/PaymentGW/GetTransaction");

            if (collection["PelecardStatusCode"] == SUCCESS_CODE)
            {
                //Validate Transact
                //var resValidate = PostJson<ValidateTransactReq, int>(new ValidateTransactReq()
                //{
                //    ConfirmationKey = collection["ConfirmationKey"],
                //    Total = res.ResultData.DebitTotal,
                //    UniqueKey = collection["UserKey"]
                //},
                //"https://gateway20.pelecard.biz/PaymentGW/ValidateByUniqueKey");

                Success = _PurchaseSrv.SaveNewPurchase(int.Parse(res.UserData.UserData7), 
                    User.GetUser(), double.Parse(res.ResultData.DebitTotal) / 100, 
                    Guid.Parse(res.UserData.UserData8)).Success;
            }
            else
            {
                Success = false;
                errMsg = "ארעה שגיאה";

                // Create purchase details to get url from pelecard
                PurchaseDetails details = new PurchaseDetails()
                {
                    Cost = double.Parse(res.ResultData.DebitTotal) / 100,
                    Customer = User.GetUser(),
                    Music = new Model.Music() { HebrewName = res.UserData.UserData5, ID = int.Parse(res.UserData.UserData7) },
                    PermissionValueNames = res.UserData.UserData6.Split(','),
                    Permission = new Model.Permission() { ID = Guid.Parse(res.UserData.UserData8) }
                };
                
                url = _GetUrl(details).URL;
            }
            
            return PartialView(ViewBag.Res = new
            {
                success = Success,
                UrlRefferer = url,
                errMessage = errMsg
            });
        }
        
        private RES PostJson<REQ, RES>(REQ request, string url)
        {
            // Creating the JSON object and encode it to bytes (The encoding choice here is the default for the application/x-www-form-urlencoded contentType).
            var jsonRequestSerializer = new DataContractJsonSerializer(typeof(REQ));

            var jsonMemoryStream = new MemoryStream();

            jsonRequestSerializer.WriteObject(jsonMemoryStream, request);

            jsonMemoryStream.Position = 0;

            var requestJsonString = (new StreamReader(jsonMemoryStream).ReadToEnd()).Replace(@"\/", "/");

            var jsonRequestBytes = Encoding.UTF8.GetBytes(requestJsonString);

            // Create the request object for the initial request.

            var jsonWebRequest = WebRequest.Create(url);
            jsonWebRequest.ContentType = "application/x-www-form-urlencoded";
            jsonWebRequest.ContentLength = jsonRequestBytes.Length;
            jsonWebRequest.Method = "POST";

            // Send the JSON data using the request object.
            using (var jsonRequestStream = jsonWebRequest.GetRequestStream())
            {
                jsonRequestStream.Write(jsonRequestBytes, 0, jsonRequestBytes.Length);
                jsonRequestStream.Close();
            }

            // Read the response JSON and decode it.
            var jsonResponse = jsonWebRequest.GetResponse();

            using (var jsonResponseStream = new StreamReader(jsonResponse.GetResponseStream()))
            {
                var responseJsonString = jsonResponseStream.ReadToEnd();

                var jsonResponseSerializer = new DataContractJsonSerializer(typeof(RES));

                jsonMemoryStream = new MemoryStream(Encoding.UTF8.GetBytes(responseJsonString));

                return (RES)jsonResponseSerializer.ReadObject(jsonMemoryStream);
            }
        }
    }
}