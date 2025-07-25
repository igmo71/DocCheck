namespace DocCheck.Common
{
    public interface IHasInvoice
    {
        public string? InvoiceRefKey { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}