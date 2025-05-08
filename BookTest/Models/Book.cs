using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookTest.Models
{
    [Table("Book")]
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "Tên sách")]
        public string BookName { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "Tác giả")]
        public string AuthorName { get; set; }
        [Display(Name = "Giá")]
        public double Price { get; set; }
        [Display(Name = "Hình ảnh")]
        public string? Image { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }

        [NotMapped]
        public string GenreName { get; set; }
    }
}
