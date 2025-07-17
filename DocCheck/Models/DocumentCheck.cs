using DocCheck.Data;

namespace DocCheck.Models
{
    public class DocumentCheck
    {
        public Guid Id { get; set; }


        public Guid DocumentId { get; set; }
        public Document? Document { get; set; }


        public Guid CheckedById { get; set; }
        public ApplicationUser? CheckedBy { get; set; }


        public Guid CheckTypeId { get; set; }
        public CheckType? CheckType { get; set; }


        public DateTime CheckedAt { get; set; }

        public bool IsValid { get; set; }
    }
}
