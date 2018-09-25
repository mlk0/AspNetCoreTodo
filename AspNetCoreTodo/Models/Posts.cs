using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models {
    public class Post {
        [Required]
        public int userId { get; set; }

        [Key]
        public int id { get; set; }

        [Required]
        [StringLength (255)]
        public string title { get; set; }

        [Required]
        [StringLength (1024)]
        public string body { get; set; }

    }
}