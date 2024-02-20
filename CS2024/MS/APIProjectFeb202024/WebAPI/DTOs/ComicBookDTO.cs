using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    //in the DTO important to retain the names
    //makes it easy to use the Auto Mapper 
    public class ComicBookDTO
    {
        public int ComicBookId { get; set; }
        public string ComicBookTitle { get; set; }
        public int ComicBookYearOfRelease { get; set; }
        public string ComicBookISBN { get; set; }
    }
}
