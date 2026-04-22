using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace API_LM.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required]
        [MaxLength(10)]
        public string DocumentType { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(80)]
        public string LastName { get; set; } = string.Empty;

        [Column(TypeName = "date")]
        public DateOnly BirthDate { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(120)]
        public string? Email { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}