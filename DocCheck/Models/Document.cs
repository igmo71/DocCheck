using DocCheck.Models.OData;
using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? DocumentRefKey { get; set; } // Счет-фактура выданный Ref_Key

        public Document_СчетФактураВыданный? DocumentRef { get; set; } // Счет-фактура выданный 


        [MaxLength(AppSettings.NAME_LENGTH)]
        public string? Barcode { get; set; }


        public Guid StatusId { get; set; }
        public DocumentStatus? Status { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime ScannedAt { get; set; }


        public List<DocumentCheck>? Checks { get; set; }

        public List<DocumentRejection>? Rejects { get; set; }
    }
}
