using DocCheck.Data;
using DocCheck.Models.OData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocCheck.Models
{
    public class DocumentCheck
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DocumentStatus? Status { get; set; } = DocumentStatus.New;

        public DocumentHandler? Handler { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public List<DocumentError>? Errors { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? DocumentRefKey { get; set; } // Счет-фактура выданный Ref_Key
        
        [NotMapped]
        [JsonIgnore]
        public Document_СчетФактураВыданный? DocumentRef { get; set; } // Счет-фактура выданный 

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
