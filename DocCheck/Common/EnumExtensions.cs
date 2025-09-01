using System.ComponentModel;
using System.Reflection;

namespace DocCheck.Common
{
    public static class EnumExtensions
    {
        public static string Description(this Enum value) =>
            value.GetType().GetField(value.ToString())?
                    .GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    }
}
