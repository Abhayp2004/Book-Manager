using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBookManager.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author cannot be longer than 100 characters")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters")]
        public string Genre { get; set; }

        [Display(Name = "Publication Year")]
        public int? PublicationYear { get; set; }

        [StringLength(20, ErrorMessage = "ISBN cannot be longer than 20 characters")]
        public string ISBN { get; set; }

        [Display(Name = "Read Status")]
        public bool IsRead { get; set; }

        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Display(Name = "Date Read")]
        [DataType(DataType.Date)]
        public DateTime? DateRead { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot be longer than 500 characters")]
        public string Notes { get; set; }
    }
}