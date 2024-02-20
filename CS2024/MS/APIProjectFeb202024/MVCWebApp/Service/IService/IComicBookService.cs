

using MVCWebApp.DTOs;

namespace MVCWebApp.Service.IService
{
    public interface IComicBookService
    {
        Task<ResponseDTO?> GetComicBookAsync(string ComicBookCode);
        Task<ResponseDTO?> GetAllComicBooksAsync();
        Task<ResponseDTO?> GetComicBookByIdAsync(int id);
        Task<ResponseDTO?> CreateComicBooksAsync(ComicBookDTO ComicBookDto);
        Task<ResponseDTO?> UpdateComicBooksAsync(ComicBookDTO ComicBookDto);
        Task<ResponseDTO?> DeleteComicBooksAsync(int id);
    }
}
