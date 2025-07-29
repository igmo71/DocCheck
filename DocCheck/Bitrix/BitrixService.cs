using DocCheck.Data;
using Microsoft.AspNetCore.Identity;

namespace DocCheck.Bitrix
{
    public class BitrixService(
        BitrixClient bitrixClient,
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        ILogger<BitrixService> logger)
    {
        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool lockoutOnFailure = false)
        {
            var bitrixUser = await GetUserAsync(userName, password);

            if (bitrixUser == null || string.IsNullOrEmpty(bitrixUser.EMAIL))
                return SignInResult.Failed;

            var appUser = await userStore.FindByNameAsync(bitrixUser.EMAIL, CancellationToken.None);

            if (appUser == null)
                await RegisterUserAsync(bitrixUser, password);

            var result = await signInManager.PasswordSignInAsync(
                userName: bitrixUser.EMAIL,
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

            var httpContent = new FormUrlEncodedContent(bitrixAuthParams);

            var authUri = configuration["Bitrix:AuthUri"];

            var authResponse = await bitrixClient.PostDataAsync<BitrixAuthResponse>(authUri, httpContent);

            var bitrixUser = authResponse?.User;

            return bitrixUser;
        }

        private async Task RegisterUserAsync(BitrixUser bitrixUser, string password)
        {
            var user = CreateUser();

            user.FirstName = bitrixUser.NAME;
            user.MiddleName = bitrixUser.SECOND_NAME;
            user.LastName = bitrixUser.LAST_NAME;
            user.BitrixId = bitrixUser.ID;

            await userStore.SetUserNameAsync(user, bitrixUser.EMAIL, CancellationToken.None);

            var emailStore = GetEmailStore();

            await emailStore.SetEmailAsync(user, bitrixUser.EMAIL, CancellationToken.None);

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                logger.LogError("{Source} {Operation} {@Errors}",
                    nameof(RegisterUserAsync), nameof(userManager.CreateAsync), result.Errors);
                return;
            }

            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                result = await userManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                    logger.LogError("{Source} {Operation} {@Errors}",
                        nameof(RegisterUserAsync), nameof(userManager.ConfirmEmailAsync), result.Errors);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor.");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)userStore;
        }
    }
}
