using DocCheck.Data;
using DocCheck.Infrastructure.OData.Models;
using DocCheck.Infrastructure.Whs.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocCheck.Domain
{
    public class SaleDoc //Document_РеализацияТоваровУслуг
    {
        [Key]
        public Guid Id { get; set; } // Ref_Key

        [MaxLength(36)]
        public string? Number { get; set; }

        public DateTime? Date { get; set; }

        public string? UserId { get; set; }

        [JsonIgnore]
        public ApplicationUser? User { get; set; }

        public Position Position { get; set; } = Position.ForDispatch;

        public int Redispatch { get; set; } // Повторная отправка

        public List<PaperworkError> PaperworkErrors { get; set; } = [];

        public List<QuantityError> QuantityErrors { get; set; } = [];

        public bool HasPaperworkErrors => PaperworkErrors.Count > 0;

        public bool HasQuantityErrors => QuantityErrors is not null && QuantityErrors.Count > 0;

        public bool IsCorrect => !HasPaperworkErrors && !HasQuantityErrors;

        public bool IsIncorrect => !IsCorrect;

        public bool ContainsError(PaperworkErrorType errorType) => PaperworkErrors.Any(e => e.Type == errorType);

        public PaperworkError? GetError(PaperworkErrorType errorType) => PaperworkErrors?.FirstOrDefault(e => e.Type == errorType);

        public string ShortDate => Date is null ? string.Empty : ((DateTime)Date).ToShortDateString();

        public bool IsOverdue => (int)(DateTime.Today - (Date ?? new DateTime()).Date).TotalDays > 5;

        public static SaleDoc From(MngrOrder? mngrOrder)
        {// Реализация товаров и услуг КСУТ-006288 от 16.04.2025 10:46:59
            if (mngrOrder == null)
                throw new ArgumentNullException(nameof(mngrOrder));

            var saleDoc = new SaleDoc
            {
                Id = Guid.Parse(mngrOrder.Распоряжение_Id),
                Number = mngrOrder.Распоряжение_Name?.Substring(27, 11)
            };

            if (DateTime.TryParse(mngrOrder.Распоряжение_Name?[42..], out DateTime dateTime))
                saleDoc.Date = dateTime;

            return saleDoc;
        }

        public static SaleDoc From(Document_РеализацияТоваровУслуг document)
        {
            var saleDoc = new SaleDoc
            {
                Id = Guid.Parse(document.Ref_Key ?? throw new InvalidOperationException("Document_РеализацияТоваровУслуг Ref_Key is null")),
                Number = document.Number,
                Date = document.Date
            };

            return saleDoc;
        }

        public string CorrectButtomLabel => Position switch
        {
            Position.Operators => "В Бухгалтерию к приемке",
            Position.Managers => "К повторной отправке",
            Position.Accounting => "Принять",
            Position.Closed => "К повторной отправке",
            _ => string.Empty
        };
    }
}
