using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocCheck.Bitrix
{
    public class BitrixService(BitrixClient bitrixClient, IConfiguration configuration)
    {

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            //var bitrixParams = new BitrixParams() { USER_LOGIN = userName, USER_PASSWORD = password };

            var bitrixAuthParams = new Dictionary<string, string>
            {
                ["USER_LOGIN"] = userName,
                ["USER_PASSWORD"] = password,
                ["AUTH_FORM"] = "Y",
                ["TYPE"] = "AUTH"
            };

            HttpContent contentForm = new FormUrlEncodedContent(bitrixAuthParams);

            var authUri = configuration["Bitrix:AuthUri"];

            var bitrixResult = await bitrixClient.PostDataAsync<BitrixAuthResponse>(authUri, contentForm);

            SignInResult result = bitrixResult.Result ? SignInResult.Success : SignInResult.Failed;

            return result;
        }

        public bool LoginAsync(string userName, string password, out BitrixUser? bitrixUser)
        {
            var bitrixAuthParams = new Dictionary<string, string>
            {
                ["USER_LOGIN"] = userName,
                ["USER_PASSWORD"] = password,
                ["AUTH_FORM"] = "Y",
                ["TYPE"] = "AUTH"
            };

            HttpContent contentForm = new FormUrlEncodedContent(bitrixAuthParams);

            var authUri = configuration["Bitrix:AuthUri"];

            var authResponse = bitrixClient.PostDataAsync<BitrixAuthResponse>(authUri, contentForm).GetAwaiter().GetResult();

            bitrixUser = authResponse?.User;

            return authResponse?.Result is bool result ? result : false;
        }
    }
}
