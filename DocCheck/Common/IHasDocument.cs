namespace DocCheck.Common
{
    public interface IHasDocument
    {
        public string? RefKey { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
    }
}