using DocCheck.Services;
using Microsoft.AspNetCore.Identity;

namespace DocCheck.Bitrix
{
    public class BitrixService(
        BitrixClient bitrixClient,
        IConfiguration configuration,
        AuthService authService)
    {
        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure = false)
        {
            var bitrixUser = await GetUserAsync(userName, password);

            if (bitrixUser == null || string.IsNullOrEmpty(bitrixUser.EMAIL))
                return SignInResult.Failed;

            //var appUser = await userStore.FindByNameAsync(bitrixUser.EMAIL, CancellationToken.None);

            var appUser = await authService.FindByEmailAsync(bitrixUser.EMAIL);

            if (appUser == null)
                await authService.RegisterUserAsync(bitrixUser, password);

            var result = await authService.PasswordSignInAsync(bitrixUser.EMAIL, password, rememberMe, lockoutOnFailure);

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

            var httpContent = new FormUrlEncodedContent(bitrixAuthParams);

            var authUri = configuration["Bitrix:AuthUri"];

            var authResponse = await bitrixClient.PostDataAsync<BitrixAuthResponse>(authUri, httpContent);

            var bitrixUser = authResponse?.User;

            return bitrixUser;
        }
    }
}
