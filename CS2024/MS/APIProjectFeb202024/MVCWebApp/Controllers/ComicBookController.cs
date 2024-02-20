using Microsoft.AspNetCore.Mvc;
using MVCWebApp.DTOs;
using MVCWebApp.Service.IService;
using Newtonsoft.Json;

namespace MVCWebApp.Controllers
{
    public class ComicBookController : Controller
    {
        private readonly IComicBookService _comicbookService;
        public ComicBookController(IComicBookService comicbookService)
        {
            _comicbookService = comicbookService;
        }
        public async Task<IActionResult> ComicBookIndex()
        {
            List<ComicBookDTO>? list = new();

            ResponseDTO? response = await _comicbookService.GetAllComicBooksAsync();

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "ComicBook list has been loaded successfully";
                list = JsonConvert.DeserializeObject<List<ComicBookDTO>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(list);
        }

        public async Task<IActionResult> ComicBookCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ComicBookCreate(ComicBookDTO model)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _comicbookService.CreateComicBooksAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "ComicBook created successfully";
                    return RedirectToAction(nameof(ComicBookIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ComicBookDelete(int comicbookId)
        {
            ResponseDTO? response = await _comicbookService.GetComicBookByIdAsync(comicbookId);

            if (response != null && response.IsSuccess)
            {
                ComicBookDTO? model = JsonConvert.DeserializeObject<ComicBookDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ComicBookDelete(ComicBookDTO comicbookDto)
        {
            ResponseDTO? response = await _comicbookService.DeleteComicBooksAsync(comicbookDto.ComicBookId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "ComicBook deleted successfully";
                return RedirectToAction(nameof(ComicBookIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(comicbookDto);
        }

        //TODO I just noticed there is no update option. but there is an update API endpoint, right? 
    }
}
