using DocCheck.Common;
using DocCheck.Data;
using DocCheck.Infrastructure.OData.Models;
using DocCheck.Infrastructure.Whs.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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


        public int PositionId { get; set; }

        public Position Position => Position.GetById(PositionId);

        public int Redispatch { get; set; } // Повторная отправка

        public List<PaperworkError> PaperworkErrors { get; set; } = [];

        public List<QuantityError> QuantityErrors { get; set; } = [];

        public bool HasPaperworkErrors => PaperworkErrors.Count > 0;

        public bool HasQuantityErrors => QuantityErrors is not null && QuantityErrors.Count > 0;

        public bool IsCorrect => !HasPaperworkErrors && !HasQuantityErrors;

        public bool IsIncorrect => !IsCorrect;

        public bool ContainsError(PaperworkErrorType errorType) => PaperworkErrors.Any(e => e.Type == errorType);

        public PaperworkError? GetError(PaperworkErrorType errorType) => PaperworkErrors?.FirstOrDefault(e => e.Type == errorType);

        public string DateString => Date is null ? string.Empty : ((DateTime)Date).ToString(new CultureInfo("ru-RU"));

        public string ShortDateString => Date is null ? string.Empty : ((DateTime)Date).ToString("d", new CultureInfo("ru-RU"));

        public bool IsOverdue => (int)(DateTime.Today - (Date ?? new DateTime()).Date).TotalDays > 5;

        public static SaleDoc From(MngrOrder? mngrOrder)
        {
            ArgumentNullException.ThrowIfNull(mngrOrder);

            var name = mngrOrder.Распоряжение_Name
                       ?? throw new ArgumentNullException(nameof(mngrOrder.Распоряжение_Name));

            var idx = name.LastIndexOf(" от ", StringComparison.Ordinal);
            if (idx == -1)
                throw new InvalidOperationException($"Can't find 'от' in: {name}");

            var numberPart = name.Substring(0, idx).Split(' ').Last();

            var datePart = name.Substring(idx + 4);

            if (!DateTime.TryParseExact(datePart, "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                throw new InvalidOperationException($"Can't parse Date from: {name}");

            return new SaleDoc
            {
                Id = Guid.Parse(mngrOrder.Распоряжение_Id),
                Number = numberPart,
                Date = dateTime
            };
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
    }
}
