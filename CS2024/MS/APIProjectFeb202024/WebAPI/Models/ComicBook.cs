using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class ComicBook
    {
        [Key]
        public int ComicBookId { get; set; }
        [Required]
        public string ComicBookTitle { get; set; }
        [Required]
        public string ComicBookISBN { get; set; }
        public int ComicBookYearOfRelease { get; set; }
    }
}
