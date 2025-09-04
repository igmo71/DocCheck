
namespace DocCheck.Common
{
    public record Position(int Id, string Role, string Description, string SubmitLabel)
    {
        public static Position Undefine { get; } = new(-1, "Undefine", "Не опркеделен", "");
        public static Position ForDispatch { get; } = new(0, "ForDispatch", "К отправке", "");
        public static Position Operators { get; } = new(1, "Operators", "Операторы РЦ", "В Бухгалтерию к приемке");
        public static Position Managers { get; } = new(2, "Managers", "Менеджеры", "К повторной отправке");
        public static Position Accounting { get; } = new(3, "Accounting", "Бухгалтерия к приемке", "Принять");
        public static Position Closed { get; } = new(4, "Closed", "Закрыт", "К повторной отправке");

        internal static Position GetById(int id) => id switch
        {
            -1 => Undefine,
            0 => ForDispatch,
            1 => Operators,
            2 => Managers,
            3 => Accounting,
            4 => Closed,
            _ => throw new ArgumentException($"Position with id {id} not found")
        };
    }
}
