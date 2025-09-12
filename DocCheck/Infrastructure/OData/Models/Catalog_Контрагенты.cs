
namespace DocCheck.Infrastructure.OData.Models
{
    public class Catalog_Контрагенты
    {
        public string? Ref_Key { get; set; }
        public string? Description { get; set; }

        public static string GetUriBySearchTerm(string searchTerm) =>
            $"Catalog_Контрагенты" +
            $"?$format=json" +
            $"&$select=Ref_Key,Description" +
            $"&$filter=substringof('{searchTerm}', Description) eq true " +
            $"&$orderby=Description";

        internal static string GetUri(string refKey) =>
            $"Catalog_Контрагенты" +
            $"?$format=json" +
            $"&$select=Ref_Key,Description" +
            $"&$filter=Ref_Key eq guid'{refKey}'";
    }
}
