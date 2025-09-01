using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocCheck.Domain
{
    public class PaperworkError
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SaleDocId { get; set; }

        [JsonIgnore]
        public SaleDoc? SaleDoc { get; set; }

        public PaperworkErrorType Type { get; set; }

        [MaxLength(450)]
        public string? Message { get; set; }
    }
}
