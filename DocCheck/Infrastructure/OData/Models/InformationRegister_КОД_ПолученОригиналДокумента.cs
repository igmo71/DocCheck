
using System.Text.Json;

namespace DocCheck.Infrastructure.OData.Models
{
    public class InformationRegister_КОД_ПолученОригиналДокумента
    {
        public string? Документ_Key { get; set; }
        public bool ЕстьДокументы { get; set; }

        internal static string GetUri(string docRefKey) =>
            $"InformationRegister_КОД_ПолученОригиналДокумента" +
            $"?$format=json" +
            $"&$filter=Документ_Key eq guid'{docRefKey}'";

        internal static string PatchUri(string docRefKey) =>
            $"InformationRegister_КОД_ПолученОригиналДокумента(guid'{docRefKey}')" +
            $"?$format=json";

        internal static string PostUri() =>
            $"InformationRegister_КОД_ПолученОригиналДокумента" +
            $"?$format=json";
    }
}
