namespace DocCheck.Infrastructure.OData
{
    public record ODataConfig(
        string BaseAddress,
        string Uri,
        string UserName,
        string Password);
}
