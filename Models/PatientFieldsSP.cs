using System.ComponentModel.DataAnnotations.Schema;

namespace API_LM.Models
{
    [NotMapped]
    public class PatientFieldsSP
    {
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
