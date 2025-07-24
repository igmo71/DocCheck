using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DocCheck.Models
{

    public enum DocumentStatus
    {
        [Description("Новый")]
        New,

        [Description("В работе")]
        InProcess,

        [Description("Закрыт")]
        Closed
    }
}
