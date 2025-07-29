using DocCheck.Data;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocCheck.Bitrix
{
    public class BitrixService(
        BitrixClient bitrixClient, 
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> UserStore,
        SignInManager<ApplicationUser> SignInManager
        )
    {

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure = false)
        {
            var bitrixUser = await GetUserAsync(userName, password);

            var result = await SignInManager.PasswordSignInAsync(
                userName: bitrixUser?.EMAIL ?? string.Empty, 
                password: password, 
                isPersistent: rememberMe, 
                lockoutOnFailure: lockoutOnFailure);

            return result;
        }

        public async Task<BitrixUser?> GetUserAsync(string userName, string password)
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

            var authResponse = await bitrixClient.PostDataAsync<BitrixAuthResponse>(authUri, contentForm);

            var bitrixUser = authResponse?.User;

            return bitrixUser;
        }

        public async Task<string> GetUserEmail(string userName, string password)
        {
            var bitrixUser = await GetUserAsync(userName, password);

            return bitrixUser?.EMAIL ?? string.Empty;
        }
    }
}
