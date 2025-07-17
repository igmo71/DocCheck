using DocCheck.Data;
using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{
    public class DocumentRejection
    {
        [Key]
        public Guid Id { get; set; }


        public Guid DocumentId { get; set; }
        public Document? Document { get; set; }


        public string? RejectedById { get; set; }
        public ApplicationUser? RejectedBy { get; set; }


        public Guid RejectionReasonId { get; set; }
        public RejectionReason? RejectionReason { get; set; }


        public DateTime RejectedAt { get; set; }
    }
}
