using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftStore.Models
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "it should be less than 500 alphabets.")]
        public string Content { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        // ربط testimonial بالمستخدم الذي أضافه
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }


        public bool IsApproved { get; set; } // خاصية جديدة للموافقة

    }
}
