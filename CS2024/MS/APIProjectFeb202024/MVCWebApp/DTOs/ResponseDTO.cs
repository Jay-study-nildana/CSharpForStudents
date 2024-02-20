namespace MVCWebApp.DTOs
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;  //by default we assume it is true
        public string Message { get; set; } = "All good";
    }
}
