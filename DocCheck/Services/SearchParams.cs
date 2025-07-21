namespace DocCheck.Services
{
    public class SearchParams
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime? Date { get; set; }
        public string? Select { get; set; }
        public string? Expand { get; set; }

        public bool IsInlinecount { get; set; }

        public bool HasFilterValue => 
            Ref_Key is not null || 
            Number is not null || 
            Date is not null;
    }
}
