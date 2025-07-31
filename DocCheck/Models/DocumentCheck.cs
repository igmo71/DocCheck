using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Models.OData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocCheck.Models
{
    public class DocumentCheck : IHasId, IHasInvoice
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Status? Status { get; set; } = Models.Status.Draft;

        public Position? Position { get; set; } = Models.Position.Operator;

        public string? UserId { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        public List<Error>? Errors { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? InvoiceRefKey { get; set; } // Счет-фактура выданный Ref_Key

        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? InvoiceNumber { get; set; } // Счет-фактура выданный Number

        public DateTime InvoiceDate { get; set; } // Счет-фактура выданный Date

        public bool IsInvoiceCorrection { get; set; }

        [NotMapped]
        public Document_СчетФактураВыданный_ДокументыОснования[]? InvoiceBaseDocuments { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Document_РеализацияТоваровУслуг>? SaleDocuments { get; set; }

        [NotMapped]
        [JsonIgnore]
        public List<Document_КорректировкаРеализации>? CorrectionDocuments { get; set; }

        [NotMapped]
        [JsonIgnore]
        public Document_СчетФактураВыданный? Document { get; set; } // Счет-фактура выданный 

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
