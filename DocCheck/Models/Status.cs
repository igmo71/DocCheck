using System.ComponentModel;

namespace DocCheck.Models
{

    public enum Status
    {
        [Description("Черновик")]
        Draft,

        [Description("В работе")]
        InProcess,

        [Description("Закрыт")]
        Closed
    }
}
