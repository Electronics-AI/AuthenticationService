using System.Collections.Generic;

namespace AuthenticationService.Web.Configurations
{
    public class WebHstsConfiguration
    {
        public bool Preload { get; set; }
        public bool IncludeSubDomains { get; set; }
        public int MaxAgeDays { get; set; }
        public List<string> ExcludedHosts { get; set; }
        public int HttpsRedirectionStatusCode { get; set; }
        public int HttpsPort { get; set; }
    }
}