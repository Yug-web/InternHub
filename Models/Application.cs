using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InternHub.Models
{
    /// <summary>
    /// Represents an internship application made by a student.
    /// Links to a Company via foreign key.
    /// </summary>
    public class Application
    {
        public int Id { get; set; }

        // Foreign Key — links this application to a Company
        [Required(ErrorMessage = "Please select a company.")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(100, ErrorMessage = "Role cannot exceed 100 characters.")]
        [Display(Name = "Role / Position")]
        public string Role { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Range(0, 200000, ErrorMessage = "Stipend must be between 0 and 2,00,000.")]
        [Display(Name = "Stipend (₹/month)")]
        public int? Stipend { get; set; }

        [Required(ErrorMessage = "Application Date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Display(Name = "Deadline")]
        public DateTime? Deadline { get; set; }

        [Required]
        [Display(Name = "Current Status")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }

        [Display(Name = "Added On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property — gives access to Company details from an Application
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
    }
}
