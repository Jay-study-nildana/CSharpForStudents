namespace WebAPI.DTOs
{
    //a common response DTO that can be used across types
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSuccess { get; set; } = true;  //by default we assume it is true
        public string Message { get; set; } = "Excellente Presidente";
    }
}
