using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IBaseService
    {
        //Task<ResponseDTO> SendAsync(RequestDto requestDto); //before tokens were added to the calling apis
        Task<ResponseDTO?> SendAsync(RequestDto requestDto, bool withBearer = true); //now tokens will get attached to api calls

    }
}
