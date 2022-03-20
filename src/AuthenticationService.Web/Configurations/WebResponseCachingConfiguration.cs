namespace AuthenticationService.Web.Configurations
{
    public class WebResponseCachingConfiguration
    {
        public int MaximumBodySizeBytes { get; set; }
        public int CacheSizeLimitBytes { get; set; }
        public bool UseCaseSensitivePaths { get; set; }
    }
}