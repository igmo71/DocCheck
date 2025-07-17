using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class Document
    {
        public Guid Id { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public required string Ref_Key { get; set; }


        [MaxLength(AppSettings.NAME_LENGTH)]
        public required string Barcode { get; set; }


        public Guid StatusId { get; set; }
        public DocumentStatus? Status { get; set; }


        public DateTime CreatedAt { get; set; }

        public DateTime ScannedAt { get; set; }
    }
}
