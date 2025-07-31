using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class Error
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DocumentCheckId { get; set; }
        public DocumentCheck? DocumentCheck { get; set; }

        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? RefKey { get; set; }
        public int LineNumber { get; set; }
        public double Quantity { get; set; }
        public TypicalError TypicalError { get; set; }
        public string? Message { get; set; }
    }
}
