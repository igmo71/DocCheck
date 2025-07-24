using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Models.OData;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocCheck.Models
{
    public class DocumentCheck : IHasDate, IHasNumber, IHasRefKey
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Status? Status { get; set; } = Models.Status.New;

        public Position? Position { get; set; }
                
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public List<Error>? Errors { get; set; }


        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? RefKey { get; set; } // Счет-фактура выданный Ref_Key

        [MaxLength(AppSettings.GUID_LENGTH)]
        public string? Number { get; set; } // Счет-фактура выданный Number

        public DateTime Date { get; set; } // Счет-фактура выданный Date

        [NotMapped]
        [JsonIgnore]
        public Document_СчетФактураВыданный? Document { get; set; } // Счет-фактура выданный 

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
