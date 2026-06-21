using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternHub.Models
{
    /// <summary>
    /// Represents an interview round for an internship application.
    /// Links to a Company via foreign key.
    /// </summary>
    public class Interview
    {
        public int Id { get; set; }

        // Foreign Key — which company is this interview at?
        [Required(ErrorMessage = "Please select a company.")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(100)]
        [Display(Name = "Role / Position")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Interview Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Interview Date")]
        public DateTime InterviewDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Interview Type")]
        public InterviewType InterviewType { get; set; } = InterviewType.Technical;

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters.")]
        [Display(Name = "Interview Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Added On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}
