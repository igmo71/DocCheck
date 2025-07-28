namespace DocCheck.Bitrix
{
    public class BitrixParams
    {
        public string? USER_LOGIN { get; set; }
        public string? USER_PASSWORD { get; set; }
        public string? AUTH_FORM { get; set; } = "Y";
        public string? TYPE { get; set; } = "AUTH";
    }
}
