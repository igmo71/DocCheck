using System.Text.Json.Serialization;

namespace DocCheck.Infrastructure.Bitrix
{
    public class BitrixAuthResponse
    {
        [JsonPropertyName("result")]
        public bool Result { get; set; }

        [JsonPropertyName("user")]
        public BitrixUser? User { get; set; }
    }

    public class BitrixUser
    {
        public string? ID { get; set; }
        public string? TIMESTAMP_X { get; set; }
        public string? LOGIN { get; set; }
        public string? PASSWORD { get; set; }
        public string? CHECKWORD { get; set; }
        public string? ACTIVE { get; set; }
        public string? NAME { get; set; }
        public string? LAST_NAME { get; set; }
        public string? EMAIL { get; set; }
        public string? LAST_LOGIN { get; set; }
        public string? DATE_REGISTER { get; set; }
        public string? LID { get; set; }
        public string? PERSONAL_PROFESSION { get; set; }
        public string? PERSONAL_WWW { get; set; }
        public string? PERSONAL_ICQ { get; set; }
        public string? PERSONAL_GENDER { get; set; }
        public string? PERSONAL_BIRTHDATE { get; set; }
        public string? PERSONAL_PHOTO { get; set; }
        public string? PERSONAL_PHONE { get; set; }
        public string? PERSONAL_FAX { get; set; }
        public string? PERSONAL_MOBILE { get; set; }
        public string? PERSONAL_PAGER { get; set; }
        public string? PERSONAL_STREET { get; set; }
        public string? PERSONAL_MAILBOX { get; set; }
        public string? PERSONAL_CITY { get; set; }
        public string? PERSONAL_STATE { get; set; }
        public string? PERSONAL_ZIP { get; set; }
        public string? PERSONAL_COUNTRY { get; set; }
        public string? PERSONAL_NOTES { get; set; }
        public string? WORK_COMPANY { get; set; }
        public string? WORK_DEPARTMENT { get; set; }
        public string? WORK_POSITION { get; set; }
        public string? WORK_WWW { get; set; }
        public string? WORK_PHONE { get; set; }
        public string? WORK_FAX { get; set; }
        public string? WORK_PAGER { get; set; }
        public string? WORK_STREET { get; set; }
        public string? WORK_MAILBOX { get; set; }
        public string? WORK_CITY { get; set; }
        public string? WORK_STATE { get; set; }
        public string? WORK_ZIP { get; set; }
        public string? WORK_COUNTRY { get; set; }
        public string? WORK_PROFILE { get; set; }
        public string? WORK_LOGO { get; set; }
        public string? WORK_NOTES { get; set; }
        public string? ADMIN_NOTES { get; set; }
        public string? STORED_HASH { get; set; }
        public string? XML_ID { get; set; }
        public string? PERSONAL_BIRTHDAY { get; set; }
        public string? EXTERNAL_AUTH_ID { get; set; }
        public string? CHECKWORD_TIME { get; set; }
        public string? SECOND_NAME { get; set; }
        public string? CONFIRM_CODE { get; set; }
        public string? LOGIN_ATTEMPTS { get; set; }
        public string? LAST_ACTIVITY_DATE { get; set; }
        public string? AUTO_TIME_ZONE { get; set; }
        public string? TIME_ZONE { get; set; }
        public string? TIME_ZONE_OFFSET { get; set; }
        public string? TITLE { get; set; }
        public string? BX_USER_ID { get; set; }
        public string? LANGUAGE_ID { get; set; }
        public string? BLOCKED { get; set; }
        public string? PASSWORD_EXPIRED { get; set; }
        public string? IS_ONLINE { get; set; }
    }
}
