using DocCheck.OData;

namespace DocCheck.Models.OData
{
    public class Document_СчетФактураВыданный
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
        public bool Корректировочный { get; set; }

        public Document_СчетФактураВыданный_ДокументыОснования[]? ДокументыОснования { get; set; }

        public static ODataParams ODataParams => new()
        {
            Select = "Ref_Key,Number,Date,Корректировочный,ДокументыОснования/ДокументОснование,ДокументыОснования/ДокументОснование_Type",
            Inlinecount = "allpages"
        };

        //public string? DataVersion { get; set; }
        //public bool DeletionMark { get; set; }
        //public bool Posted { get; set; }
        //public string? Организация_Key { get; set; }
        //public string? Контрагент { get; set; }
        //public string? Контрагент_Type { get; set; }
        //public string? ДокументОснование { get; set; }
        //public string? ДокументОснование_Type { get; set; }
        //public string? СчетФактураОснование_Key { get; set; }
        //public string? СтрокаПлатежноРасчетныеДокументы { get; set; }
        //public bool Исправление { get; set; }
        //public string? НомерИсправления { get; set; }
        //public string? Валюта_Key { get; set; }
        //public bool ВыставленВЭлектронномВиде { get; set; }
        //public DateTime ДатаВыставления { get; set; }
        //public string? КодВидаОперации { get; set; }
        //public string? ИдентификаторПлатежа { get; set; }
        //public string? КППКонтрагента { get; set; }
        //public string? КодВидаОперацииНаУменьшение { get; set; }
        //public string? Подразделение_Key { get; set; }
        //public string? Ответственный_Key { get; set; }
        //public bool СводныйКорректировочный { get; set; }
        //public string? ИННКонтрагента { get; set; }
        //public string? ИдентификаторГосКонтракта { get; set; }
        //public string? НаправлениеДеятельности_Key { get; set; }
        //public string? Партнер_Key { get; set; }
        //public string? Договор { get; set; }
        //public string? Договор_Type { get; set; }
        //public string? Склад_Key { get; set; }
        //public string? ПредставлениеНомера { get; set; }
        //public string? НалогообложениеНДС { get; set; }
        //public bool РучнаяКорректировкаСуммДокумента { get; set; }
        //public string? Руководитель_Key { get; set; }
        //public string? ГлавныйБухгалтер_Key { get; set; }
        //public bool РучнаяКорректировкаЖурналаСФ { get; set; }
        //public string? Комментарий { get; set; }
        //public bool Перевыставленный { get; set; }
        //public bool РеализацияЧерезКомиссионера { get; set; }
        //public string? Автор_Key { get; set; }
        //public bool НДССМежценовойРазницы { get; set; }
        //public string? СозданНаОсновании_Key { get; set; }
        //public СчетФактураВыданный_ПлатежноРасчетныеДокументы[]? ПлатежноРасчетныеДокументы { get; set; }
        //public object[]? Товары { get; set; }
        //public object[]? Покупатели { get; set; }
        //public string? ОрганизацияnavigationLinkUrl { get; set; }
        //public string? ВалютаnavigationLinkUrl { get; set; }
        //public string? ОтветственныйnavigationLinkUrl { get; set; }
        //public string? СчетФактураОснованиеnavigationLinkUrl { get; set; }
    }

    public class Document_СчетФактураВыданный_ДокументыОснования
    {
        public string? Ref_Key { get; set; }
        public int LineNumber { get; set; }
        public string? ДокументОснование { get; set; }
        public string? ДокументОснование_Type { get; set; }

        //public string? ХозяйственнаяОперация { get; set; }
        //public string? СчетФактураПолученныйОтПродавца_Key { get; set; }
        //public string? ПорядковыеНомераСтрок { get; set; }
    }

    //public class СчетФактураВыданный_ПлатежноРасчетныеДокументы
    //{
    //    public string? Ref_Key { get; set; }
    //    public string? LineNumber { get; set; }
    //    public string? НомерПлатежноРасчетногоДокумента { get; set; }
    //    public DateTime ДатаПлатежноРасчетногоДокумента { get; set; }
    //}


}
