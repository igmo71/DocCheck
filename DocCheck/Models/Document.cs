using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? Ref_Key { get; set; }


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
