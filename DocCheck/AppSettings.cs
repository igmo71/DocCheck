using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DocCheck
{
    public class AppSettings
    {
        public const int GUID_LENGTH = 36;
        public const int NAME_LENGTH = 100;

        public static readonly JsonSerializerOptions JsonSerializerOptions = new()
        {
            //ReferenceHandler = ReferenceHandler.Preserve,
            //MaxDepth = 3,
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
        };
    }
}
