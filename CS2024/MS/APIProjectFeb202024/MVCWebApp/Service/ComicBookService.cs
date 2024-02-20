using MVCWebApp.DTOs;
using MVCWebApp.Service.IService;
using MVCWebApp.Utility;

namespace MVCWebApp.Service
{
    public class ComicBookService : IComicBookService
    {
        private readonly IBaseService _baseService;
        public ComicBookService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> CreateComicBooksAsync(ComicBookDTO ComicBookDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = ComicBookDto,
                Url = SD.ComicBookAPIBase + "/api/comicbook"
            });
        }

        public async Task<ResponseDTO?> DeleteComicBooksAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ComicBookAPIBase + "/api/comicbook/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllComicBooksAsync()
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ComicBookAPIBase + "/api/comicbook"
            });
        }

        public async Task<ResponseDTO?> GetComicBookAsync(string ComicBookCode)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ComicBookAPIBase + "/api/comicbook/GetByISBN/" + ComicBookCode
            });
        }

        public async Task<ResponseDTO?> GetComicBookByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ComicBookAPIBase + "/api/comicbook/" + id
            });
        }

        public async Task<ResponseDTO?> UpdateComicBooksAsync(ComicBookDTO ComicBookDto)
        {
            return await _baseService.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.PUT,
                Data = ComicBookDto,
                Url = SD.ComicBookAPIBase + "/api/comicbook"
            });
        }
    }
}
