namespace DocCheck.Infrastructure.OData.Models
{
    public class Catalog_Пользователи
    {
        public string? Ref_Key { get; set; }
        public string? Description { get; set; }

        public static string GetSalesDepartmentUri() => 
            $"Catalog_Пользователи" +
            $"?$format=json" +
            $"&$select=Ref_Key,Description" +
            $"&$orderby=Description" +
            $"&$filter=Подразделение_Key eq guid'213925a7-0e9f-11e0-ad48-000c29dcd88a'";
    }
}
