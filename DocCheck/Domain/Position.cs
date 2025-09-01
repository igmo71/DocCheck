using System.ComponentModel;

namespace DocCheck.Domain
{
    public enum Position
    {
        [Description("К отправке")]
        ForDispatch,

        [Description("Операторы РЦ")]
        Operators,

        [Description("Менеджеры")]
        Managers,

        [Description("Бухгалтерия к приемке")]
        Accounting,

        [Description("Закрыт")]
        Closed
    }
}
