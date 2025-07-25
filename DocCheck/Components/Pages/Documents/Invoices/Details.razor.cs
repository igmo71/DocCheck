using DocCheck.Models.OData;
using DocCheck.OData;
using DocCheck.Services;
using Microsoft.AspNetCore.Components;


namespace DocCheck.Components.Pages.Documents.Invoices
{
    public partial class Details
    {
        [Inject]
        public DataSource? DataSource { get; set; }

        [SupplyParameterFromQuery]
        public required string Ref_Key { get; set; }


        private Document_СчетФактураВыданный? document;
        private List<Document_РеализацияТоваровУслуг>? saleDocuments;
        private List<Document_КорректировкаРеализации>? correctionDocuments;
        private SearchParams searchParams = new();
        private ODataParams oDataParams = Document_СчетФактураВыданный.ODataParams;

        protected override async Task OnInitializedAsync()
        {
            searchParams.RefKey = Ref_Key;

            document = await GetDocumentAsync(searchParams);

            if (document is not null)
                await GetBaseDocuments(document);
        }

        private async Task<Document_СчетФактураВыданный?> GetDocumentAsync(SearchParams searchParams)
        {
            if (DataSource == null)
                return null;

            var rootobject = await DataSource.GetDataAsync<Document_СчетФактураВыданный>(searchParams, oDataParams);

            document = rootobject?.Value?.FirstOrDefault();

            return document;
        }

        public async Task GetBaseDocuments(Document_СчетФактураВыданный document)
        {
            if (document?.ДокументыОснования is not null)
            {
                foreach (var doc in document.ДокументыОснования)
                {
                    if (!string.IsNullOrEmpty(doc.ДокументОснование_Type) && doc.ДокументОснование_Type.Contains(nameof(Document_РеализацияТоваровУслуг)))
                    {
                        saleDocuments ??= [];

                        if (await GetSaleDocument(doc.ДокументОснование) is Document_РеализацияТоваровУслуг baseDoc)
                            saleDocuments.Add(baseDoc);
                    }
                    if (!string.IsNullOrEmpty(doc.ДокументОснование_Type) && doc.ДокументОснование_Type.Contains(nameof(Document_КорректировкаРеализации)))
                    {
                        correctionDocuments ??= [];

                        if (await GetCorrectionDocument(doc.ДокументОснование) is Document_КорректировкаРеализации baseDoc)
                            correctionDocuments.Add(baseDoc);
                    }
                }
            }
        }

        private async Task<Document_РеализацияТоваровУслуг?> GetSaleDocument(string? refKey)
        {
            if (DataSource == null)
                return null;

            var rootobject = await DataSource
                .GetDataAsync<Document_РеализацияТоваровУслуг>(new SearchParams { RefKey = refKey }, Document_РеализацияТоваровУслуг.ODataParams);

            var baseDoc = rootobject?.Value?.FirstOrDefault();

            return baseDoc;
        }

        private async Task<Document_КорректировкаРеализации?> GetCorrectionDocument(string? refKey)
        {
            if (DataSource == null)
                return null;

            var rootobject = await DataSource
                .GetDataAsync<Document_КорректировкаРеализации>(new SearchParams { RefKey = refKey }, Document_КорректировкаРеализации.ODataParams);

            var baseDoc = rootobject?.Value?.FirstOrDefault();

            return baseDoc;
        }
    }
}
