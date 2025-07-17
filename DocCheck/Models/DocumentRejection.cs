using DocCheck.Data;

namespace DocCheck.Models
{
    public class DocumentRejection
    {
        public Guid Id { get; set; }


        public Guid DocumentId { get; set; }
        public Document? Document { get; set; }


        public Guid RejectedById { get; set; }
        public ApplicationUser? RejectedBy { get; set; }


        public Guid RejectionReasonId { get; set; }
        public RejectionReason? RejectionReason { get; set; }


        public DateTime RejectedAt { get; set; }
    }
}
