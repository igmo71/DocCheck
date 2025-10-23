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

        public DateTime CreatedAt { get; set; } = DateTime.Now;

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

        public bool IsOriginalDocumentReceived { get; set; }

        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }

        public string? AuthorId { get; set; }
        public string? AuthorName { get; set; }

        public string? ManagerId { get; set; }
        public string? ManagerName { get; set; }

        public Guid? ManagerTaskId { get; set; }

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

        public static SaleDoc From(Document_СчетФактураВыданный documentInvoice, Document_РеализацияТоваровУслуг documentBase)
        {
            var saleDoc = new SaleDoc
            {
                Id = Guid.Parse(documentInvoice.Ref_Key ?? throw new InvalidOperationException("Document_СчетФактураВыданный Ref_Key is null")),
                Number = documentInvoice.Number,
                Date = documentInvoice.Date,
                BaseDocId = documentInvoice.ДокументОснование,
                BaseDocType = documentInvoice.ДокументОснование_Type,
                CustomerId = documentBase.Контрагент_Key,
                CustomerName = documentBase.Контрагент?.Description,
                AuthorId = documentBase.Автор_Key,
                AuthorName = documentBase.Автор?.Description,
                ManagerId = documentBase.Менеджер_Key,
                ManagerName = documentBase.Менеджер?.Description
            };

            return saleDoc;
        }

        internal string GetErrorDetails()
        {
            string result = string.Empty;

            foreach(var error in PaperworkErrors)
            {
                result += $"\n{error.Type.Description()}";
                if (!string.IsNullOrEmpty(error.Message))
                    result += $": {error.Message}";
            }

            return result;
        }

        internal string? GetTaskHandler(string taskHandler)
        {
            if (taskHandler.Equals("Manager") && !string.IsNullOrEmpty(ManagerId))
                return ManagerId;

            if (taskHandler.Equals("Author") && !string.IsNullOrEmpty(AuthorId))
                return AuthorId;

            //return "ad310d19-7b3f-11ea-8148-0cc47adeb013"; // Могильницкий Игорь Анатольевич ))
            return null;
        }
    }
}
