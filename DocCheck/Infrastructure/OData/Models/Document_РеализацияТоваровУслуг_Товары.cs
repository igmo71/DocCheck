namespace DocCheck.Infrastructure.OData.Models
{
    public class Document_РеализацияТоваровУслуг_Товары
    {
        public string? Ref_Key { get; set; }
        public int LineNumber { get; set; }
        public double Количество { get; set; }

        public string? Номенклатура_Key { get; set; }
        public Catalog_Номенклатура? Номенклатура { get; set; }

        public static string GetUri(string refKey) =>
            $"Document_РеализацияТоваровУслуг_Товары" +
            $"?$format=json" +
            $"&$expand=Номенклатура" +
            $"&$select=Ref_Key,LineNumber,Количество,Номенклатура_Key,Номенклатура/Description" +
            $"&$filter=Ref_Key eq guid'{refKey}'";

        //public string Характеристика_Key { get; set; }
        //public string Назначение_Key { get; set; }
        //public string Упаковка_Key { get; set; }
        //public int КоличествоУпаковок { get; set; }
        //public string ВидЦены_Key { get; set; }
        //public float Цена { get; set; }
        //public float Сумма { get; set; }
        //public string СтавкаНДС_Key { get; set; }
        //public float СуммаНДС { get; set; }
        //public float СуммаСНДС { get; set; }
        //public string КодСтроки { get; set; }
        //public float СуммаРучнойСкидки { get; set; }
        //public int СуммаАвтоматическойСкидки { get; set; }
        //public int ПроцентРучнойСкидки { get; set; }
        //public int ПроцентАвтоматическойСкидки { get; set; }
        //public string КлючСвязи { get; set; }
        //public string Склад_Key { get; set; }
        //public int СтатусУказанияСерий { get; set; }
        //public float СуммаВзаиморасчетов { get; set; }
        //public string ЗаказКлиента { get; set; }
        //public string ЗаказКлиента_Type { get; set; }
        //public string СрокПоставки { get; set; }
        //public string ИдентификаторСтроки { get; set; }
        //public string Серия_Key { get; set; }
        //public string АналитикаУчетаНоменклатуры_Key { get; set; }
        //public string НоменклатураНабора_Key { get; set; }
        //public string ХарактеристикаНабора_Key { get; set; }
        //public string АналитикаУчетаНаборов_Key { get; set; }
        //public string КодТНВЭД_Key { get; set; }
        //public string ОбъектРасчетов_Key { get; set; }
        //public string Подразделение_Key { get; set; }
        //public string НоменклатураПартнера_Key { get; set; }
        //public int СуммаБонусныхБалловКСписанию { get; set; }
        //public int СуммаБонусныхБалловКСписаниюВВалюте { get; set; }
        //public int СуммаНачисленныхБонусныхБалловВВалюте { get; set; }
    }
}
