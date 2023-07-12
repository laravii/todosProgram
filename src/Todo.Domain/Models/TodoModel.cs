using System.ComponentModel.DataAnnotations;

namespace Todo.Domain.Models
{
    public class TodoModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int DaysToFinish { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public int Status { get; set; }
    }
}
