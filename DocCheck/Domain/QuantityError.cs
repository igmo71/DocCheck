using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocCheck.Domain
{
    public class QuantityError
    {
        [Key]
        public Guid Id { get; set; }

        public Guid SaleDocId { get; set; }

        [JsonIgnore]
        public SaleDoc? SaleDoc { get; set; }

        public int LineNumber { get; set; }

        public double Quantity { get; set; }
        [MaxLength(450)]

        public string? Message { get; set; }
    }
}
