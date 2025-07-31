using System.ComponentModel;

namespace DocCheck.Models
{
    public enum TypicalError
    {
        [Description("Прочее")]
        Other = -1,

        [Description("Нет печати")]
        NoStamp = 1,

        [Description("Нет подписи")]
        NoSignature = 2
    }
}
