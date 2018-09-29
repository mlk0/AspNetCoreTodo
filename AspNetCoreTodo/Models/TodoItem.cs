using System;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models {
    public class TodoItem {
        public Guid Id { get; set; }

        public bool IsDone { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType (DataType.Date)]
        [DisplayFormat (DataFormatString = "{yyyy-MM-dd}")]
        public DateTimeOffset? DueAt { get; set; }
        public string UserId { get; internal set; }
    }
}