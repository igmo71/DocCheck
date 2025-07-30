using DocCheck.Models;

namespace DocCheck.Services
{
    public class SearchParams
    {
        public string? RefKey { get; set; }
        public string? Number { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string? UserId { get; set; }
        public bool IsShowClosed { get; set; } = false;

        public int Skip { get; set; }
        public int? Take { get; set; }
        public string? OrderBy { get; set; }
        public bool IsOrderAsc { get; set; }

        public bool HasFilterValue => 
            RefKey is not null || 
            Number is not null || 
            Date is not null;
    }
}
