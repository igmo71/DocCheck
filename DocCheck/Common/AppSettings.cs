using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DocCheck.Common
{
    public class AppSettings
    {
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
