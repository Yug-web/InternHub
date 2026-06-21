using System.ComponentModel.DataAnnotations;

namespace InternHub.Models
{
    /// <summary>
    /// Represents a company that a student may apply to for an internship.
    /// </summary>
    public class Company
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Industry is required.")]
        [StringLength(50, ErrorMessage = "Industry cannot exceed 50 characters.")]
        public string Industry { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; } = string.Empty;

        [Url(ErrorMessage = "Please enter a valid URL (e.g. https://example.com).")]
        [StringLength(200)]
        public string? Website { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(100)]
        [Display(Name = "HR Email")]
        public string? HREmail { get; set; }

        [Display(Name = "Added On")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties (one company → many applications/interviews)
        public ICollection<Application> Applications { get; set; } = new List<Application>();
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
