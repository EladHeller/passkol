using System.Web;

namespace Infrastructure.Helper
{
    public static class CommonHelper
    {
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
                

            if (!string.IsNullOrWhiteSpace(appUrl) && appUrl != "/")
                appUrl += "/";


            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);
            return baseUrl;
        }
    }
}