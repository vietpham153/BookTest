using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTest.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "Thể loại")]
        public string GenreName { get; set; }
        public List<Book> Books { get; set; }
    }
}
