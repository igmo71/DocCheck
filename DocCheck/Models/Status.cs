using System.ComponentModel;

namespace DocCheck.Models
{

    public enum Status
    {
        [Description("Новый")]
        New,

        [Description("В работе")]
        InProcess,

        [Description("Закрыт")]
        Closed
    }
}
