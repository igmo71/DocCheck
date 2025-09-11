using System.Globalization;

namespace DocCheck.Infrastructure.OData.Models
{

    public class Document_РеализацияТоваровУслуг
    {
        public string? Ref_Key { get; set; }
        public string? Number { get; set; }
        public DateTime Date { get; set; }
        public decimal СуммаДокумента { get; set; }

        public Document_РеализацияТоваровУслуг_Товары[]? Товары { get; set; }

        public string? Автор_Key { get; set; }
        public Catalog_Пользователи? Автор { get; set; }

        public string? Менеджер_Key { get; set; }
        public Catalog_Пользователи? Менеджер { get; set; }

        public string? Контрагент_Key { get; set; }
        public Catalog_Контрагенты? Контрагент { get; set; }

        public string DocumentAmount => СуммаДокумента.ToString("N2", new CultureInfo("ru-RU"));

        public static string DocumentName => "Реализация товаров и услуг";

        public static string GetUri(string refKey) =>
            $"Document_РеализацияТоваровУслуг" +
            $"?$format=json" +
            $"&$expand=Менеджер,Автор,Контрагент" +
            $"&$select=Ref_Key,Number,Date,СуммаДокумента,Менеджер/Description,Автор/Description,Контрагент/Description" +
            $"&$filter=Ref_Key eq guid'{refKey}'";

        //public string DataVersion { get; set; }
        //public bool DeletionMark { get; set; }
        //public bool Posted { get; set; }
        //public string АдресДоставки { get; set; }
        //public string БанковскийСчетОрганизации_Key { get; set; }
        //public string БанковскийСчетКонтрагента_Key { get; set; }
        //public string БанковскийСчетГрузоотправителя_Key { get; set; }
        //public string БанковскийСчетГрузополучателя_Key { get; set; }
        //public string Валюта_Key { get; set; }
        //public string ВалютаВзаиморасчетов_Key { get; set; }
        //public string Грузоотправитель_Key { get; set; }
        //public string Грузополучатель_Key { get; set; }
        //public string ДоверенностьВыдана { get; set; }
        //public DateTime ДоверенностьДата { get; set; }
        //public string ДоверенностьЛицо { get; set; }
        //public string ДоверенностьНомер { get; set; }
        //public string ЗаказКлиента { get; set; }
        //public string ЗаказКлиента_Type { get; set; }
        //public string Организация_Key { get; set; }
        //public string НалогообложениеНДС { get; set; }
        //public string Партнер_Key { get; set; }
        //public string Подразделение_Key { get; set; }
        //public string Сделка_Key { get; set; }
        //public bool СкидкиРассчитаны { get; set; }
        //public string Склад_Key { get; set; }
        //public bool Согласован { get; set; }
        //public string Соглашение_Key { get; set; }
        //public float СуммаВзаиморасчетов { get; set; }
        //public string Комментарий { get; set; }
        //public string ФормаОплаты { get; set; }
        //public string ХозяйственнаяОперация { get; set; }
        //public bool ЦенаВключаетНДС { get; set; }
        //public string Касса_Key { get; set; }
        //public string Отпустил_Key { get; set; }
        //public string ОтпустилДолжность { get; set; }
        //public bool РеализацияПоЗаказам { get; set; }
        //public string ГруппаФинансовогоУчета_Key { get; set; }
        //public string КартаЛояльности_Key { get; set; }
        //public string Договор_Key { get; set; }
        //public string Основание { get; set; }
        //public string Статус { get; set; }
        //public string СпособДоставки { get; set; }
        //public string ЗонаДоставки_Key { get; set; }
        //public string АдресДоставкиЗначенияПолей { get; set; }
        //public string ПеревозчикПартнер_Key { get; set; }
        //public DateTime ВремяДоставкиС { get; set; }
        //public DateTime ВремяДоставкиПо { get; set; }
        //public string АдресДоставкиПеревозчика { get; set; }
        //public string АдресДоставкиПеревозчикаЗначенияПолей { get; set; }
        //public string ДополнительнаяИнформацияПоДоставке { get; set; }
        //public string КонтактноеЛицо_Key { get; set; }
        //public string Руководитель_Key { get; set; }
        //public string ГлавныйБухгалтер_Key { get; set; }
        //public string ПорядокРасчетов { get; set; }
        //public bool ВернутьМногооборотнуюТару { get; set; }
        //public DateTime ДатаВозвратаМногооборотнойТары { get; set; }
        //public string СостояниеЗаполненияМногооборотнойТары { get; set; }
        //public bool ВидыЗапасовУказаныВручную { get; set; }
        //public bool ТребуетсяЗалогЗаТару { get; set; }
        //public DateTime ОснованиеДата { get; set; }
        //public string ОснованиеНомер { get; set; }
        //public string ДопоставкаПоРеализации { get; set; }
        //public string ДопоставкаПоРеализации_Type { get; set; }
        //public DateTime ДатаПереходаПраваСобственности { get; set; }
        //public string ВариантОформленияПродажи { get; set; }
        //public string ИдентификаторПлатежа { get; set; }
        //public bool ОсобыеУсловияПеревозки { get; set; }
        //public string ОсобыеУсловияПеревозкиОписание { get; set; }
        //public string НаправлениеДеятельности_Key { get; set; }
        //public int КурсЧислитель { get; set; }
        //public int КурсЗнаменатель { get; set; }
        //public bool ЕстьМаркируемаяПродукцияГИСМ { get; set; }
        //public int СуммаВзаиморасчетовПоТаре { get; set; }
        //public bool ОплатаВВалюте { get; set; }
        //public string Курьер_Key { get; set; }
        //public string Сборщик_Key { get; set; }
        //public string АдресДоставкиЗначение { get; set; }
        //public string АдресДоставкиПеревозчикаЗначение { get; set; }
        //public string ВариантВыбытияМаркируемойПродукции { get; set; }
        //public string КлиентКонтрагент_Key { get; set; }
        //public string КлиентПартнер_Key { get; set; }
        //public string КлиентДоговор_Key { get; set; }
        //public string ЭтапГосконтрактаЕИС { get; set; }
        //public string ОбъектРасчетовУпр_Key { get; set; }
        //public string ГрафикОплаты_Key { get; set; }
        //public DateTime ИШ_ЖелаемаяДатаДоставки { get; set; }
        //public string ИШ_ДоставкаКонтактноеЛицо { get; set; }
        //public string ИШ_ДоставкаТелефон { get; set; }
        //public string ИШ_ТипВыгрузки { get; set; }
        //public DateTime ИШ_НачалоОбеда { get; set; }
        //public DateTime ИШ_КонецОбеда { get; set; }
        //public bool САР_Корректировка { get; set; }
        //public object[] СкидкиНаценки { get; set; }
        //public Видызапасов[] ВидыЗапасов { get; set; }
        //public object[] ДополнительныеРеквизиты { get; set; }
        //public Расшифровкаплатежа[] РасшифровкаПлатежа { get; set; }
        //public object[] Серии { get; set; }
        //public Этапыграфикаоплаты[] ЭтапыГрафикаОплаты { get; set; }
        //public object[] ШтрихкодыУпаковок { get; set; }
        //public object[] НачислениеБонусныхБаллов { get; set; }
        //public string БанковскийСчетОрганизацииnavigationLinkUrl { get; set; }
        //public string ВалютаnavigationLinkUrl { get; set; }
        //public string ВалютаВзаиморасчетовnavigationLinkUrl { get; set; }
        //public string ОрганизацияnavigationLinkUrl { get; set; }
        //public string КонтрагентnavigationLinkUrl { get; set; }
        //public string МенеджерnavigationLinkUrl { get; set; }
        //public string ПартнерnavigationLinkUrl { get; set; }
        //public string ПодразделениеnavigationLinkUrl { get; set; }
        //public string СкладnavigationLinkUrl { get; set; }
        //public string СоглашениеnavigationLinkUrl { get; set; }
        //public string КассаnavigationLinkUrl { get; set; }
        //public string ДоговорnavigationLinkUrl { get; set; }
        //public string АвторnavigationLinkUrl { get; set; }
        //public string РуководительnavigationLinkUrl { get; set; }
        //public string ГлавныйБухгалтерnavigationLinkUrl { get; set; }
    }

    //public class Видызапасов
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string АналитикаУчетаНоменклатуры_Key { get; set; }
    //    public string ВидЗапасов_Key { get; set; }
    //    public string НомерГТД_Key { get; set; }
    //    public string Упаковка_Key { get; set; }
    //    public int КоличествоУпаковок { get; set; }
    //    public int Количество { get; set; }
    //    public int КоличествоПоРНПТ { get; set; }
    //    public float СуммаСНДС { get; set; }
    //    public string СтавкаНДС_Key { get; set; }
    //    public float СуммаНДС { get; set; }
    //    public string ИдентификаторСтроки { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public string ЗаказКлиента { get; set; }
    //    public string ЗаказКлиента_Type { get; set; }
    //    public float СуммаРучнойСкидки { get; set; }
    //    public int СуммаАвтоматическойСкидки { get; set; }
    //    public string АналитикаУчетаНаборов_Key { get; set; }
    //    public float Цена { get; set; }
    //    public string ВидЗапасовПолучателя_Key { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //    public string КодТНВЭД_Key { get; set; }
    //}

    //public class Расшифровкаплатежа
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public float Сумма { get; set; }
    //    public string ВалютаВзаиморасчетов_Key { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //}

    //public class Этапыграфикаоплаты
    //{
    //    public string Ref_Key { get; set; }
    //    public string LineNumber { get; set; }
    //    public string Заказ { get; set; }
    //    public string Заказ_Type { get; set; }
    //    public bool СверхЗаказа { get; set; }
    //    public string ВариантОплаты { get; set; }
    //    public DateTime ДатаПлатежа { get; set; }
    //    public string Сдвиг { get; set; }
    //    public float СуммаПлатежа { get; set; }
    //    public int ПроцентПлатежа { get; set; }
    //    public int СуммаЗалогаЗаТару { get; set; }
    //    public int ПроцентЗалогаЗаТару { get; set; }
    //    public float СуммаВзаиморасчетов { get; set; }
    //    public int СуммаВзаиморасчетовПоТаре { get; set; }
    //    public string ОбъектРасчетов_Key { get; set; }
    //    public string ВариантОтсчета { get; set; }
    //}
}
