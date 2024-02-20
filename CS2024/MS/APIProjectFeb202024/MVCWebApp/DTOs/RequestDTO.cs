using static MVCWebApp.Utility.SD;

namespace MVCWebApp.DTOs
{
    public class RequestDTO
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;  //for the token jwt adding process
    }
}
