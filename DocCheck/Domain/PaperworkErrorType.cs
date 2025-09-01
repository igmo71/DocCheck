using System.ComponentModel;

namespace DocCheck.Domain
{
    public enum PaperworkErrorType
    {
        [Description("Прочее")]
        Other,


        [Description("Нет документа")]
        NoDocument,


        // Печать
        [Description("нет Печати (нужна)")]
        NoStamp,

        [Description("не та Печать")]
        WrongStamp,


        // Грузополучатель
        [Description("нет Должности")]
        ConsigneeJobTitle,

        [Description("нет/не та Подпись")]
        ConsigneeSignature,

        [Description("нет/не то ФИО")]
        ConsigneeFullName,

        [Description("нет/не та Дата получения")]
        ReceiptCargoDate,

        [Description("нет Доверенности на право получения")]
        ConsigneePowerOfAttorney,

        [Description("нет/не тот Адрес получения")]
        ReceiptAddress,


        // Ответственный за оформление
        [Description("нет Должности")]
        ResponsibleJobTitle,

        [Description("нет/не та Подпись")]
        ResponsibleSignature,

        [Description("нет/не то ФИО")]
        ResponsibleFullName,

        [Description("нет Доверенности на право оформления")]
        ResponsiblePowerOfAttorney,


        [Description("Ошибки состава товаров")]
        QuantityError
    }
}
