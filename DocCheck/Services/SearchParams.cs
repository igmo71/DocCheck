namespace DocCheck.Services
{
    public class SearchParams
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime? Date { get; set; }

        public bool HasAnyValue => Ref_Key is not null || Number is not null || Date is not null;
    }
}
