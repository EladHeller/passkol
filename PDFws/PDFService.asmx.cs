using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.html;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.css;

namespace PDFws
{
    /// <summary>
    /// Summary description for PDFService
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]    [WebService(Namespace = "http://tempuri.org/")]

    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PDFService : System.Web.Services.WebService
    {
        private const string USER_CONV = "User-";
        private const char splitIndicator = '^';
        private const string ARTIST_AGREEMENT_HTML_FILE_Temp = "ArtistAgreementTemplate.html";
        private const string ARTIST_AGREEMENT_HTML_FILE = "ArtistAgreement.html";
        private const string ARTIST_EDIT_MUSIC_HTML_FILE = "EditMusicTemp.html";
        private const string PURCHASE_AGREEMENT_HTML_FILE = "PurchaseAgreementTemplate.html";
        private const string PURCHASE_AGREEMENT_HTML_FILE_WITHOUT_EXEPTIONS =
            "PurchaseAgreementTemplateWithoutException.html";
        private const string ARTIST_AGREEMENT_FN = "ArtistAgreement_V0.pdf";
        private void ValidateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        
        [WebMethod]
        public void createPDF(string UserId, string BaseRoute, string text, string fileName)
        {
            string directory = Path.Combine(BaseRoute, "PDF", USER_CONV + UserId);
            ValidateDirectory(directory);
            string path = Path.Combine(directory, fileName);
            Document doc = new Document();
            try
            {
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();
                
                text = text.Replace(Environment.NewLine, String.Empty).Replace("  ", String.Empty);

                BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/fonts/nrkis.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                PdfPTable table = new PdfPTable(1);
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                PdfPCell cell = new PdfPCell();
                table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
                Font nrkis = new Font(bf, 12, Font.BOLD);
                cell.AddElement(new Paragraph(text, nrkis));
                table.AddCell(cell);
                doc.Add(table);
            }
            catch (DocumentException dex)
            {
                throw (dex);
            }
            catch (IOException ioex)
            {
                throw (ioex);
            }
            finally
            {
                doc.Close();
            }
        }

        [WebMethod]
        public void ArtistPermissionAgreement(DateTime Time, string name, string email, string UserId, 
            string BaseRoute)
        {
            // base user directory
            string BaseDirectory = Path.Combine(BaseRoute, "PDF", USER_CONV + UserId);
            ValidateDirectory(BaseDirectory);

            // html template for artist agreement
            string HtmlSourcePath = HttpContext.Current.Server.MapPath(
                string.Format("~/HTML/{0}", ARTIST_AGREEMENT_HTML_FILE_Temp));

            // html destination for user agreement
            string HtmlDestPath = Path.Combine(BaseDirectory, ARTIST_AGREEMENT_HTML_FILE);

            // PDF destination for user agreement
            string PdfDestPath = Path.Combine(BaseDirectory, ARTIST_AGREEMENT_FN);

            
            string[] texts = { Time.Day.ToString(), Time.Month.ToString(), Time.Year.ToString(),
                name, name, email };

            // create html and get its content if html create successfuly then create pdf from it
            string htmlTemplate = GenerateHtmlWithPH(HtmlSourcePath, HtmlDestPath, texts, splitIndicator);
            if (!string.IsNullOrEmpty(htmlTemplate))
            {
                CreatePdfFromHtmlString(htmlTemplate, PdfDestPath);
            }
        }

        [WebMethod]
        public void ArtistAddMusic(DateTime Time, string MusicName,string Composer, string Singer, 
            string Exeptions, string Writer, string UserId, string BaseRoute)
        {
            // base user directory
            string BaseDirectory = Path.Combine(BaseRoute, "PDF", USER_CONV + UserId);
            ValidateDirectory(BaseDirectory);

            // html template for artist agreement
            string HtmlSourcePath = HttpContext.Current.Server.MapPath(
                string.Format("~/HTML/{0}", ARTIST_EDIT_MUSIC_HTML_FILE));

            // html destination for user agreement
            string HtmlDestPath = Path.Combine(BaseDirectory, ARTIST_AGREEMENT_HTML_FILE);

            // PDF destination for user agreement
            string PdfDestPath = Path.Combine(BaseDirectory, ARTIST_AGREEMENT_FN);


            string[] texts = { Time.Day.ToString(), Time.Month.ToString(), Time.Year.ToString(),Time.ToShortTimeString(),
                MusicName, Writer, Composer, Singer, Exeptions};

            // create html and get its content if html create successfuly then create pdf from it

            // append html temp to html
            string newHtmlTemp = HtmlFileWithPHToString(HtmlSourcePath, texts, splitIndicator);
            File.AppendAllText(HtmlDestPath, newHtmlTemp);

            string htmlTemplate = File.ReadAllText(HtmlDestPath);
            if (!string.IsNullOrEmpty(htmlTemplate))
            {
                CreatePdfFromHtmlString(htmlTemplate, PdfDestPath);
            }
        }

        [WebMethod]
        public string GetTempPurchaseHtml(
            DateTime time,
            string customerName,
            string email,
            string musicName,
            string musicWriter,
            string musicComposer,
            string musicPerformer,
            string permission,
            double cost,
            string reference,
            string exceptions
            )
        {
            string[] texts = null;
            string htmlTemplateName = null;

            if (string.IsNullOrWhiteSpace(exceptions))
            {
                texts = new string[] { time.Day.ToString(), time.Month.ToString(), time.Year.ToString(),
                customerName, email, musicName,musicWriter,musicComposer,musicPerformer,permission,
                cost.ToString(),reference};
                htmlTemplateName = PURCHASE_AGREEMENT_HTML_FILE_WITHOUT_EXEPTIONS;
            }
            else
            {
                texts = new string[] { time.Day.ToString(), time.Month.ToString(), time.Year.ToString(),
                customerName, email, musicName,musicWriter,musicComposer,musicPerformer,permission,
                cost.ToString(),reference,exceptions};
                htmlTemplateName = PURCHASE_AGREEMENT_HTML_FILE;
            }

            // html template for artist agreement
            string HtmlSourcePath = HttpContext.Current.Server.MapPath(
            string.Format("~/HTML/{0}", htmlTemplateName));

            string htmlTemplate = HtmlFileWithPHToString(HtmlSourcePath, texts, splitIndicator);
            
            return htmlTemplate;
        }


        [WebMethod]
        public void PurchaseAgreement(
            string BaseRoute, 
            string FileId,
            string UserId, 
            DateTime time,
            string customerName,
            string email,
            string musicName,
            string musicWriter,
            string musicComposer,
            string musicPerformer,
            string permission,
            double cost,
            string reference,
            string exceptions
            )
        {
            string[] texts = null;
            string htmlTemplateName = null;

            if (string.IsNullOrWhiteSpace(exceptions))
            {
                texts = new string[] { time.Day.ToString(), time.Month.ToString(), time.Year.ToString(),
                customerName, email, musicName,musicWriter,musicComposer,musicPerformer,permission,
                cost.ToString(),reference};
                htmlTemplateName = PURCHASE_AGREEMENT_HTML_FILE_WITHOUT_EXEPTIONS;
            }
            else
            {
                texts = new string[] { time.Day.ToString(), time.Month.ToString(), time.Year.ToString(),
                customerName, email, musicName,musicWriter,musicComposer,musicPerformer,permission,
                cost.ToString(),reference,exceptions};
                htmlTemplateName = PURCHASE_AGREEMENT_HTML_FILE;
            }

            // base user directory
            string BaseDirectory = Path.Combine(BaseRoute, "PDF", USER_CONV + UserId);
            ValidateDirectory(BaseDirectory);

            // html template for artist agreement
            string HtmlSourcePath = HttpContext.Current.Server.MapPath(
            string.Format("~/HTML/{0}", htmlTemplateName));

            // html destination for user agreement
            string HtmlDestPath = Path.Combine(BaseDirectory, Path.GetFileNameWithoutExtension(FileId) + ".html");

            // PDF destination for user agreement
            string PdfDestPath = Path.Combine(BaseDirectory, FileId);

            string htmlTemplate = GenerateHtmlWithPH(HtmlSourcePath, HtmlDestPath, texts, splitIndicator);
            if (!string.IsNullOrEmpty(htmlTemplate))
            {
                CreatePdfFromHtmlString(htmlTemplate, PdfDestPath);
            }
        }

        private string GenerateHtmlWithPH(string HtmlSourcefileName, string HtmlDestinationName,
            string[] text, char PHSplit)
        {
            string template = string.Empty;
            var htmlTemp = HtmlFileWithPHToString(HtmlSourcefileName, text, PHSplit);

            if (!string.IsNullOrEmpty(htmlTemp))
            {
                try
                {
                    File.WriteAllText(HtmlDestinationName, htmlTemp);
                    template = htmlTemp;
                }
                catch (Exception e)
                {
                }
            }

            return template;
        }

        private string HtmlFileWithPHToString(string HtmlSourcefileName, string[] text, char PHSplit)
        {
            string[] htmlLines = File.ReadAllText(HtmlSourcefileName, Encoding.UTF8).Split(PHSplit);
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < htmlLines.Length - 1; i ++)
            {
                sb.Append(htmlLines[i] + text[i]);
            }
            sb.Append(htmlLines[htmlLines.Length - 1]);
            return sb.ToString();
        }
        
        private void CreatePdfFromHtmlString(string HtmlTemplate, string DestinationPath)
        {
            Document doc = new Document();

            var fontFile = Server.MapPath("~/fonts/ARIALUNI.ttf");
            try
            {
                var FS = new System.IO.FileStream(DestinationPath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

                var document = new Document();

                var writer = PdfWriter.GetInstance(document, FS);
            
                document.Open();

                var cssResolver = new StyleAttrCSSResolver();
                var fontProvider = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
                fontProvider.Register(fontFile);
                var cssAppliers = new CssAppliersImpl(fontProvider);
                var htmlContext = new HtmlPipelineContext(cssAppliers);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

                var pdf = new PdfWriterPipeline(document, writer);
                var html = new HtmlPipeline(htmlContext, pdf);
                var css = new CssResolverPipeline(cssResolver, html);
                
                var worker = new XMLWorker(css, true);
                var p = new XMLParser(worker);

                var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(HtmlTemplate));

                var sr = new StreamReader(ms);
                              
                p.Parse(sr);

                document.Close();
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
