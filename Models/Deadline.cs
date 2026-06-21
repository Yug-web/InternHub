using System.ComponentModel.DataAnnotations;

namespace InternHub.Models
{
    /// <summary>
    /// Represents an important deadline (e.g., application deadline, test date).
    /// </summary>
    public class Deadline
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(7);

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Required]
        public Priority Priority { get; set; } = Priority.Medium;

        [Display(Name = "Added On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Computed property: Is this deadline already past?
        public bool IsOverdue => DueDate.Date < DateTime.Now.Date;
    }
}
