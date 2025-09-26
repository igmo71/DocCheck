namespace DocCheck.Infrastructure.OData.Models;

public class OneSTask
{
    public string? Ref_Key { get; set; }
    public string? DataVersion { get; set; }
    public bool DeletionMark { get; set; }
    public string? Number { get; set; }
    public DateTime Date { get; set; }
    public string? BusinessProcess { get; set; }
    public string? BusinessProcess_Type { get; set; }
    public string? RoutePoint { get; set; }
    public string? RoutePoint_Type { get; set; }
    public string? Description { get; set; }
    public bool Executed { get; set; }
    public string? Автор { get; set; }
    public string? Автор_Type { get; set; }
    public string? Важность { get; set; }
    public string? ГруппаИсполнителейЗадач_Key { get; set; }
    public DateTime ДатаИсполнения { get; set; }
    public DateTime ДатаНачала { get; set; }
    public DateTime ДатаПринятияКИсполнению { get; set; }
    public string? Описание { get; set; }
    public string? Предмет { get; set; }
    public string? Предмет_Type { get; set; }
    public string? ПредметСтрокой { get; set; }
    public bool ПринятаКИсполнению { get; set; }
    public string? РезультатВыполнения { get; set; }
    public string? СостояниеБизнесПроцесса { get; set; }
    public DateTime СрокИсполнения { get; set; }
    public string? АвторСтрокой { get; set; }
    public string? ДополнительныйОбъектАдресации { get; set; }
    public string? ДополнительныйОбъектАдресации_Type { get; set; }
    public string? Исполнитель { get; set; }
    public string? Исполнитель_Type { get; set; }
    public string? ОсновнойОбъектАдресации { get; set; }
    public string? ОсновнойОбъектАдресации_Type { get; set; }
    public string? РольИсполнителя_Key { get; set; }
    public string? ГруппаИсполнителейЗадачnavigationLinkUrl { get; set; }

    internal static string PostUri() => $"Task_ЗадачаИсполнителя?$format=json";
}
