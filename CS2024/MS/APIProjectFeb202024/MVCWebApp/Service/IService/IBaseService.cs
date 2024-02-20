

using MVCWebApp.DTOs;

namespace MVCWebApp.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDTO> SendAsync(RequestDTO requestDto); //before tokens were added to the calling apis

    }
}
