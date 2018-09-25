using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models {
    public class Album {
        [Required]
        public int userId { get; set; }

        [Key]
        public int id { get; set; }
        public string title { get; set; }

    }
}