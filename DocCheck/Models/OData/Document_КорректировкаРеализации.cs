using DocCheck.OData;

namespace DocCheck.Models.OData
{
    public class Document_КорректировкаРеализации
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }

        public Document_КорректировкаРеализации_Товары[]? Товары { get; set; }

        public string? Автор_Key { get; set; }
        public Catalog_Пользователи? Автор { get; set; }

        public string? Менеджер_Key { get; set; }
        public Catalog_Пользователи? Менеджер { get; set; }

        public string? Контрагент_Key { get; set; }
        public Catalog_Контрагенты? Контрагент { get; set; }

        public static string Name => nameof(Document_КорректировкаРеализации).Substring(9);

        public static ODataParams ODataParams => new()
        {
            Expand = "Менеджер,Автор,Контрагент",
            Select = "Ref_Key,Number,Date,Менеджер/Description,Автор/Description,Контрагент/Description"
        };

        //public string DataVersion { get; set; }
        //public bool DeletionMark { get; set; }
        //public bool Posted { get; set; }
        //public string ДокументОснование { get; set; }
        //public string ДокументОснование_Type { get; set; }
        //public string Партнер_Key { get; set; }
        //public string Соглашение_Key { get; set; }
        //public string Организация_Key { get; set; }
        //public string Договор_Key { get; set; }
        //public string Склад_Key { get; set; }
        //public string АдресДоставкиЗначение { get; set; }
        //public string Сделка_Key { get; set; }
        //public string Валюта_Key { get; set; }
        //public string ВалютаВзаиморасчетов_Key { get; set; }
        //public float СуммаДокумента { get; set; }
        //public DateTime ДатаПлатежа { get; set; }
        //public string Подразделение_Key { get; set; }
        //public string ФормаОплаты { get; set; }
        //public bool ЦенаВключаетНДС { get; set; }
        //public bool ПродажаПоЗаказам { get; set; }
        //public string ПодразделениеДоходы_Key { get; set; }
        //public string НалогообложениеНДС { get; set; }
        //public string СтатьяДоходов_Key { get; set; }
        //public string АналитикаДоходов { get; set; }
        //public string АналитикаДоходов_Type { get; set; }
        //public string ПодразделениеРасходы_Key { get; set; }
        //public string СтатьяРасходов_Key { get; set; }
        //public string АналитикаРасходов { get; set; }
        //public string АналитикаРасходов_Type { get; set; }
        //public bool Согласован { get; set; }
        //public string Отпустил_Key { get; set; }
        //public string ОтпустилДолжность { get; set; }
        //public string Основание { get; set; }
        //public string Грузополучатель_Key { get; set; }
        //public string Грузоотправитель_Key { get; set; }
        //public string БанковскийСчетОрганизации_Key { get; set; }
        //public string БанковскийСчетКонтрагента_Key { get; set; }
        //public string БанковскийСчетГрузоотправителя_Key { get; set; }
        //public string БанковскийСчетГрузополучателя_Key { get; set; }
        //public string АдресДоставки { get; set; }
        //public string ДоверенностьНомер { get; set; }
        //public DateTime ДоверенностьДата { get; set; }
        //public string ДоверенностьВыдана { get; set; }
        //public string ДоверенностьЛицо { get; set; }
        //public string Комментарий { get; set; }
        //public string Руководитель_Key { get; set; }
        //public string ГлавныйБухгалтер_Key { get; set; }
        //public string ПорядокРасчетов { get; set; }
        //public string ВидКорректировки { get; set; }
        //public float СуммаВзаиморасчетов { get; set; }
        //public DateTime ОснованиеДата { get; set; }
        //public string ОснованиеНомер { get; set; }
        //public string АктОРасхожденияхПослеОтгрузкиОснование_Key { get; set; }
        //public string ВариантОформленияПродажи { get; set; }
        //public string НаправлениеДеятельности_Key { get; set; }
        //public int КурсЧислитель { get; set; }
        //public int КурсЗнаменатель { get; set; }
        //public string АдресДоставкиЗначенияПолей { get; set; }
        //public string ХозяйственнаяОперация { get; set; }
        //public string АдресДоставкиПеревозчикаЗначение { get; set; }
        //public string ГруппаФинансовогоУчета_Key { get; set; }
        //public string КлиентКонтрагент_Key { get; set; }
        //public string КлиентПартнер_Key { get; set; }
        //public string КлиентДоговор_Key { get; set; }
        //public string ВариантНалогообложенияПрибыли { get; set; }
        //public string ЭтапГосконтрактаЕИС { get; set; }
        //public string ТипКорректировки { get; set; }
        //public string ГрафикОплаты_Key { get; set; }
        //public string ИсправляемыйДокумент { get; set; }
        //public string ИсправляемыйДокумент_Type { get; set; }
        //public Расхождения[] Расхождения { get; set; }
        //public object[] ВидыЗапасовКорректировкаВыручки { get; set; }
        //public Видызапасовоприходование[] ВидыЗапасовОприходование { get; set; }
        //public Видызапасовсписание[] ВидыЗапасовСписание { get; set; }
        //public object[] ДополнительныеРеквизиты { get; set; }
        //public object[] ШтрихкодыУпаковок { get; set; }
        //public object[] КорректировкаЗадолженности { get; set; }
        //public object[] НачислениеБонусныхБаллов { get; set; }
        //public object[] ОплатаБонуснымиБаллами { get; set; }
        //public string ПартнерnavigationLinkUrl { get; set; }
        //public string КонтрагентnavigationLinkUrl { get; set; }
        //public string СоглашениеnavigationLinkUrl { get; set; }
        //public string ОрганизацияnavigationLinkUrl { get; set; }
        //public string СкладnavigationLinkUrl { get; set; }
        //public string ВалютаnavigationLinkUrl { get; set; }
        //public string ВалютаВзаиморасчетовnavigationLinkUrl { get; set; }
        //public string МенеджерnavigationLinkUrl { get; set; }
        //public string БанковскийСчетОрганизацииnavigationLinkUrl { get; set; }
        //public string ПодразделениеnavigationLinkUrl { get; set; }
        //public string РуководительnavigationLinkUrl { get; set; }
        //public string ГлавныйБухгалтерnavigationLinkUrl { get; set; }
        //public string АвторnavigationLinkUrl { get; set; }
        //public string ГрафикОплатыnavigationLinkUrl { get; set; }
        //public string ДоговорnavigationLinkUrl { get; set; }
        //public string БанковскийСчетКонтрагентаnavigationLinkUrl { get; set; }
        //public string АктОРасхожденияхПослеОтгрузкиОснованиеnavigationLinkUrl { get; set; }
    }

    public class Document_КорректировкаРеализации_Товары
    {
        public string? Ref_Key { get; set; }
        public int LineNumber { get; set; }
        public double Количество { get; set; }

        public string? Номенклатура_Key { get; set; }
        public Catalog_Номенклатура? Номенклатура { get; set; }

        public static ODataParams ODataParams => new()
        {
            Expand = "Номенклатура",
            Select = "Ref_Key,LineNumber,Количество,Номенклатура_Key,Номенклатура/Description",
            OrderBy = "LineNumber"
        };


        //public string Характеристика_Key { get; set; }
        //public string Назначение_Key { get; set; }
        //public string Содержание { get; set; }
        //public string Упаковка_Key { get; set; }
        //public float КоличествоУпаковок { get; set; }
        //public string ВидЦены_Key { get; set; }
        //public float Цена { get; set; }
        //public float Сумма { get; set; }
        //public string СтавкаНДС_Key { get; set; }
        //public float СуммаНДС { get; set; }
        //public float СуммаСНДС { get; set; }
        //public string ЗаказКлиента { get; set; }
        //public string ЗаказКлиента_Type { get; set; }
        //public string КодСтроки { get; set; }
        //public string Склад_Key { get; set; }
        //public string Серия_Key { get; set; }
        //public int СтатусУказанияСерий { get; set; }
        //public string ВариантОтражения { get; set; }
        //public string НоменклатураНабора_Key { get; set; }
        //public string ХарактеристикаНабора_Key { get; set; }
        //public string СтатьяДоходов_Key { get; set; }
        //public string АналитикаДоходов { get; set; }
        //public string АналитикаДоходов_Type { get; set; }
        //public string КодТНВЭД_Key { get; set; }
        //public string Подразделение_Key { get; set; }
        //public string СтатьяРасходов { get; set; }
        //public string СтатьяРасходов_Type { get; set; }
        //public string АналитикаРасходов { get; set; }
        //public string АналитикаРасходов_Type { get; set; }
        //public string АналитикаАктивовПассивов { get; set; }
        //public string АналитикаАктивовПассивов_Type { get; set; }
        //public bool СписатьНаРасходы { get; set; }
        //public string НоменклатураПартнера_Key { get; set; }
        //public string НомерГТД_Key { get; set; }
        //public int КоличествоПоРНПТ { get; set; }
        //public string ИдентификаторСтроки { get; set; }
        //public int СуммаВзаиморасчетов { get; set; }
        //public int СуммаБезНДСРегл { get; set; }
        //public int СуммаБезНДСУпр { get; set; }
    }

    //public class Расхождения
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string Номенклатура_Key { get; set; }
    //    public string Характеристика_Key { get; set; }
    //    public string Назначение_Key { get; set; }
    //    public string Содержание { get; set; }
    //    public string Упаковка_Key { get; set; }
    //    public float КоличествоУпаковок { get; set; }
    //    public float Количество { get; set; }
    //    public float Сумма { get; set; }
    //    public string СтавкаНДС_Key { get; set; }
    //    public float СуммаНДС { get; set; }
    //    public float СуммаСНДС { get; set; }
    //    public string ЗаказКлиента { get; set; }
    //    public string ЗаказКлиента_Type { get; set; }
    //    public string КодСтроки { get; set; }
    //    public string Склад_Key { get; set; }
    //    public string ВариантОтражения { get; set; }
    //    public string ИдентификаторСтроки { get; set; }
    //    public string Серия_Key { get; set; }
    //    public string АналитикаУчетаНоменклатуры_Key { get; set; }
    //    public int СтатусУказанияСерий { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public string НоменклатураНабора_Key { get; set; }
    //    public string ХарактеристикаНабора_Key { get; set; }
    //    public string АналитикаУчетаНаборов_Key { get; set; }
    //    public string СтатьяДоходов_Key { get; set; }
    //    public string АналитикаДоходов { get; set; }
    //    public string АналитикаДоходов_Type { get; set; }
    //    public string КодТНВЭД_Key { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //    public string Подразделение_Key { get; set; }
    //    public string СтатьяРасходов { get; set; }
    //    public string СтатьяРасходов_Type { get; set; }
    //    public string АналитикаРасходов { get; set; }
    //    public string АналитикаРасходов_Type { get; set; }
    //    public string АналитикаАктивовПассивов { get; set; }
    //    public string АналитикаАктивовПассивов_Type { get; set; }
    //    public bool СписатьНаРасходы { get; set; }
    //    public string НоменклатураПартнера_Key { get; set; }
    //    public string НомерГТД_Key { get; set; }
    //    public int КоличествоПоРНПТ { get; set; }
    //    public string СпособОпределенияСебестоимости { get; set; }
    //    public int Себестоимость { get; set; }
    //    public int СебестоимостьБезНДС { get; set; }
    //    public int СебестоимостьРегл { get; set; }
    //    public int СебестоимостьПР { get; set; }
    //    public int СебестоимостьВР { get; set; }
    //    public string ВидЦеныСебестоимости_Key { get; set; }
    //    public DateTime ДатаЗаполненияСебестоимостиПоВидуЦены { get; set; }
    //    public int СуммаБезНДСРегл { get; set; }
    //    public int СуммаБезНДСУпр { get; set; }
    //}

    //public class Видызапасовоприходование
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string ВидЗапасов_Key { get; set; }
    //    public string НомерГТД_Key { get; set; }
    //    public float Количество { get; set; }
    //    public int КоличествоПоРНПТ { get; set; }
    //    public float СуммаСНДС { get; set; }
    //    public string СтавкаНДС_Key { get; set; }
    //    public float СуммаНДС { get; set; }
    //    public string Склад_Key { get; set; }
    //    public string СкладОтгрузки_Key { get; set; }
    //    public string ЗаказКлиента { get; set; }
    //    public string ЗаказКлиента_Type { get; set; }
    //    public string ВидЗапасовОтгрузки_Key { get; set; }
    //    public bool НаДоходыРасходы { get; set; }
    //    public string Упаковка_Key { get; set; }
    //    public float КоличествоУпаковок { get; set; }
    //    public string ИдентификаторСтроки { get; set; }
    //    public string АналитикаУчетаНоменклатуры_Key { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public string АналитикаУчетаНаборов_Key { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //    public string КодТНВЭД_Key { get; set; }
    //    public string КорВидЗапасов_Key { get; set; }
    //    public string СтатьяРасходов { get; set; }
    //    public string СтатьяРасходов_Type { get; set; }
    //    public string АналитикаРасходов { get; set; }
    //    public string АналитикаРасходов_Type { get; set; }
    //    public string АналитикаАктивовПассивов { get; set; }
    //    public string АналитикаАктивовПассивов_Type { get; set; }
    //    public bool СписатьНаРасходы { get; set; }
    //    public string НоменклатураПартнера_Key { get; set; }
    //    public string СпособОпределенияСебестоимости { get; set; }
    //    public int Себестоимость { get; set; }
    //    public int СебестоимостьБезНДС { get; set; }
    //    public int СебестоимостьРегл { get; set; }
    //    public int СебестоимостьПР { get; set; }
    //    public int СебестоимостьВР { get; set; }
    //    public int СуммаБезНДСРегл { get; set; }
    //    public int СуммаБезНДСУпр { get; set; }
    //}

    //public class Видызапасовсписание
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string ВидЗапасов_Key { get; set; }
    //    public string НомерГТД_Key { get; set; }
    //    public int Количество { get; set; }
    //    public int КоличествоПоРНПТ { get; set; }
    //    public float СуммаСНДС { get; set; }
    //    public string СтавкаНДС_Key { get; set; }
    //    public float СуммаНДС { get; set; }
    //    public string Склад_Key { get; set; }
    //    public string ЗаказКлиента { get; set; }
    //    public string ЗаказКлиента_Type { get; set; }
    //    public bool НаДоходыРасходы { get; set; }
    //    public string Упаковка_Key { get; set; }
    //    public int КоличествоУпаковок { get; set; }
    //    public string ИдентификаторСтроки { get; set; }
    //    public string АналитикаУчетаНоменклатуры_Key { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public string АналитикаУчетаНаборов_Key { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //    public string КодТНВЭД_Key { get; set; }
    //    public string КорВидЗапасов_Key { get; set; }
    //    public string СтатьяРасходов { get; set; }
    //    public string СтатьяРасходов_Type { get; set; }
    //    public string АналитикаРасходов { get; set; }
    //    public string АналитикаРасходов_Type { get; set; }
    //    public string АналитикаАктивовПассивов { get; set; }
    //    public string АналитикаАктивовПассивов_Type { get; set; }
    //    public bool СписатьНаРасходы { get; set; }
    //    public string НоменклатураПартнера_Key { get; set; }
    //    public int СуммаБезНДСРегл { get; set; }
    //    public int СуммаБезНДСУпр { get; set; }
    //}
}
