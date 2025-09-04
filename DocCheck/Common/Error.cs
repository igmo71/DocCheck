namespace DocCheck.Common
{
    public sealed record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NotFound = new("ErrorNotFound", "Доекмент не найден");
        public static readonly Error MismatchedRole = new ("ErrorMismatchedRole", "Вы не может открыть ljrevtyn из этой колонки");
    }

}
