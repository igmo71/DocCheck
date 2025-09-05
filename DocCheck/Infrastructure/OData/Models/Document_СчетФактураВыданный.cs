namespace DocCheck.Infrastructure.OData.Models
{
    public class Document_СчетФактураВыданный
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
        public string? ДокументОснование { get; set; }
        public string? ДокументОснование_Type { get; set; }

        internal static string GetUri(string refKey) =>
            $"Document_СчетФактураВыданный" +
            $"?$format=json" +
            $"&$select=Ref_Key,Number,Date,ДокументОснование,ДокументОснование_Type" +
            $"&$filter=Ref_Key eq guid'{refKey}'";

        internal static string GetUriByDocumentSale(string baseDocRefKey) =>
            $"Document_СчетФактураВыданный" +
            $"?$format=json" +
            $"&$select=Ref_Key,Number,Date,ДокументОснование,ДокументОснование_Type" +
            $"&$filter=ДокументОснование eq cast(guid'{baseDocRefKey}', 'Document_РеализацияТоваровУслуг')";

        //public bool Корректировочный { get; set; }
        //public string Контрагент { get; set; }
        //public string Ответственный_Key { get; set; }
        //public string Автор_Key { get; set; }
        //public Document_СчетФактураВыданный_ДокументыОснования[] ДокументыОснования { get; set; }
        //public string? DataVersion { get; set; }
        //public bool DeletionMark { get; set; }
        //public bool Posted { get; set; }
        //public string Организация_Key { get; set; }
        //public string Контрагент_Type { get; set; }
        //public string СчетФактураОснование_Key { get; set; }
        //public string СтрокаПлатежноРасчетныеДокументы { get; set; }
        //public bool Исправление { get; set; }
        //public string НомерИсправления { get; set; }        
        //public string Валюта_Key { get; set; }
        //public bool ВыставленВЭлектронномВиде { get; set; }
        //public DateTime ДатаВыставления { get; set; }
        //public string КодВидаОперации { get; set; }
        //public string ИдентификаторПлатежа { get; set; }
        //public string КППКонтрагента { get; set; }
        //public string КодВидаОперацииНаУменьшение { get; set; }
        //public string Подразделение_Key { get; set; }        
        //public bool СводныйКорректировочный { get; set; }
        //public string ИННКонтрагента { get; set; }
        //public string ИдентификаторГосКонтракта { get; set; }
        //public string НаправлениеДеятельности_Key { get; set; }
        //public string Партнер_Key { get; set; }
        //public string Договор { get; set; }
        //public string Договор_Type { get; set; }
        //public string Склад_Key { get; set; }
        //public string ПредставлениеНомера { get; set; }
        //public string НалогообложениеНДС { get; set; }
        //public bool РучнаяКорректировкаСуммДокумента { get; set; }
        //public string Руководитель_Key { get; set; }
        //public string ГлавныйБухгалтер_Key { get; set; }
        //public bool РучнаяКорректировкаЖурналаСФ { get; set; }
        //public string Комментарий { get; set; }
        //public bool Перевыставленный { get; set; }
        //public bool РеализацияЧерезКомиссионера { get; set; }
        //public bool НДССМежценовойРазницы { get; set; }
        //public string СозданНаОсновании_Key { get; set; }
        //public Платежнорасчетныедокументы[] ПлатежноРасчетныеДокументы { get; set; }
        //public object[] Товары { get; set; }
        //public object[] Покупатели { get; set; }
        //public string ОрганизацияnavigationLinkUrl { get; set; }
        //public string ВалютаnavigationLinkUrl { get; set; }
        //public string ОтветственныйnavigationLinkUrl { get; set; }
    }    

    //public class Платежнорасчетныедокументы
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string НомерПлатежноРасчетногоДокумента { get; set; }
    //    public DateTime ДатаПлатежноРасчетногоДокумента { get; set; }
    //}
}
