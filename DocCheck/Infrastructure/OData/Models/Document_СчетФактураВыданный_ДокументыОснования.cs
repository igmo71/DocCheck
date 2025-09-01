namespace DocCheck.Infrastructure.OData.Models
{
    public class Document_СчетФактураВыданный_ДокументыОснования
    {
        public string? Ref_Key { get; set; }
        public int LineNumber { get; set; }
        public string? ДокументОснование { get; set; }

        public static string GetUri(string refKey) =>
            $"Document_СчетФактураВыданный_ДокументыОснования" +
            $"?$format=json" +
            $"&$select=Ref_Key,LineNumber,ДокументОснование" +
            $"&$filter=Ref_Key eq guid'{refKey}' and ДокументОснование_Type eq 'StandardODATA.Document_РеализацияТоваровУслуг'";

        //public string ДокументОснование_Type { get; set; }
        //public string ХозяйственнаяОперация { get; set; }
        //public string СчетФактураПолученныйОтПродавца_Key { get; set; }
        //public string ПорядковыеНомераСтрок { get; set; }
    }
}
