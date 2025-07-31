using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DocCheck.Models
{
    public class DocumentCheckLog
    {
        public DocumentCheckLog()
        { }

        public DocumentCheckLog(DocumentCheck documentCheck)
        {
            DateTime = DateTime.Now;
            DocumentCheckId = documentCheck.Id;
            Log = JsonSerializer.Serialize(documentCheck, AppSettings.JsonSerializerOptions);
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public Guid DocumentCheckId { get; set; }

        public string? Log { get; set; }

        [NotMapped]
        public string DocumentCheckIdString
        {
            get => DocumentCheckId.ToString();
            set { 
                if(Guid.TryParse(value, out Guid id))
                    DocumentCheckId = id;
            }
        }

        [NotMapped]
        public DocumentCheck? DocumentCheck => Log is null ? null : JsonSerializer.Deserialize<DocumentCheck>(Log);
    }
}
