namespace DocCheck.Infrastructure.RabbitMq
{
    public class RabbitMqConfig
    {
        public const string Section = "RabbitMq";

        public bool IsUse { get; set; }
        public string? HostName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public List<QueueBind> QueueBinds { get; set; } = [];

        public bool IsOk => HostName is not null && UserName is not null && Password is not null;
        public bool IsWrong => !IsOk;
    }

    public class QueueBind
    {
        public string? Exchange { get; set; }
        public string? Queue { get; set; }
        public string? RoutingKey { get; set; }

        public bool IsOk => Queue is not null && Exchange is not null && RoutingKey is not null;
        public bool IsWrong => !IsOk;
    }
}
