using DocCheck.Models.OData;

namespace DocCheck.Services
{
    public interface ICorrectionDocService
    {
        Task<Document_КорректировкаРеализации> GetItem(string? документОснование);
    }

    public class CorrectionDocService : ICorrectionDocService
    {
        public Task<Document_КорректировкаРеализации> GetItem(string? документОснование)
        {
            throw new NotImplementedException();
        }
    }
}