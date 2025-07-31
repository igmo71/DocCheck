using System.ComponentModel;

namespace DocCheck.Models
{
    public enum TypicalError
    {
        [Description("Неопределено")]
        Undefined,

        [Description("Нет печати")]
        NoStamp,

        [Description("Нет подписи")]
        NoSignature
    }
}
