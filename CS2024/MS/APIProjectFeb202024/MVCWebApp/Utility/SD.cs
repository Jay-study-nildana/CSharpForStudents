namespace MVCWebApp.Utility
{
    public class SD
    {
        public static string ComicBookAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        public enum ContentType
        {
            Json,
            MultipartFormData,
        }
    }
}
