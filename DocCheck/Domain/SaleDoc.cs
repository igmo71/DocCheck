using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Infrastructure.OData.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.Json.Serialization;

namespace DocCheck.Domain
{
    public class SaleDoc //Счет-Фактура (Invoice)
    {
        [Key]
        public Guid Id { get; set; } // Ref_Key

        [MaxLength(36)]
        public string? Number { get; set; }

        public DateTime? Date { get; set; }

        public string? BaseDocId { get; set; }
        public string? BaseDocType { get; set; }

        [NotMapped]
        public Document_РеализацияТоваровУслуг? BaseDoc { get; set; }

        public string? UserId { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }


        public int PositionId { get; set; }

        public Position Position => Position.GetById(PositionId);

        public int Redispatch { get; set; } // Повторная отправка

        public List<PaperworkError> PaperworkErrors { get; set; } = [];

        public List<QuantityError> QuantityErrors { get; set; } = [];

        ///

        public bool HasPaperworkErrors => PaperworkErrors.Count > 0;

        public bool HasQuantityErrors => QuantityErrors is not null && QuantityErrors.Count > 0;

        public bool IsCorrect => !HasPaperworkErrors && !HasQuantityErrors;

        public bool IsIncorrect => !IsCorrect;

        public bool ContainsError(PaperworkErrorType errorType) => PaperworkErrors.Any(e => e.Type == errorType);

        public PaperworkError? GetError(PaperworkErrorType errorType) => PaperworkErrors?.FirstOrDefault(e => e.Type == errorType);

        public string DateString => Date is null ? string.Empty : ((DateTime)Date).ToString(new CultureInfo("ru-RU"));

        public string ShortDateString => Date is null ? string.Empty : ((DateTime)Date).ToString("d", new CultureInfo("ru-RU"));

        public bool IsOverdue => (int)(DateTime.Today - (Date ?? new DateTime()).Date).TotalDays > 5;

        public static SaleDoc From(Document_СчетФактураВыданный documentInvoice)
        {
            var saleDoc = new SaleDoc
            {
                Id = Guid.Parse(documentInvoice.Ref_Key ?? throw new InvalidOperationException("Document_СчетФактураВыданный Ref_Key is null")),
                Number = documentInvoice.Number,
                Date = documentInvoice.Date,
                BaseDocId = documentInvoice.ДокументОснование,
                BaseDocType = documentInvoice.ДокументОснование_Type
            };

            return saleDoc;
        }
    }
}
