using DocCheck.Common;
using DocCheck.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DocCheck.Domain
{
    public class SaleDocLog
    {
        public SaleDocLog()
        { }

        public SaleDocLog(SaleDoc saleDoc)
        {
            DateTime = DateTime.Now;
            SaleDocId = saleDoc.Id;
            UserId = saleDoc.UserId;
            IsCorrect = saleDoc.IsCorrect;
            Log = JsonSerializer.Serialize(saleDoc, AppSettings.JsonSerializerOptions);
        }

        [Key]
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public bool IsCorrect { get; set; }

        public Guid SaleDocId { get; set; }

        public string? Log { get; set; }

        [NotMapped]
        public SaleDoc? SaleDoc
        {
            get
            {
                if (Log is null)
                    return null;
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<SaleDoc>(Log, options);
            }
        }

        public string Description => IsCorrect ? "Корректен" : "Требуется перепечатка";
    }
}
