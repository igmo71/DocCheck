namespace DocCheck.Common
{
    public sealed record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NotFound = new("NotFound", "Доекмент не найден");
        public static readonly Error MismatchedRole = new ("MismatchedRole", "Вы не может открыть документ из этой колонки");
    }

}
