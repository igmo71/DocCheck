using DocCheck.Bitrix;
using DocCheck.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DocCheck.Services
{
    public class AuthService(
        AuthenticationStateProvider authStateProvider,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUserStore<ApplicationUser> userStore,
        ILogger<AuthService> logger)
    {
        public async Task<string?> GetUserIdAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();

            var userId = authState?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            var appUser = await userManager.FindByEmailAsync(email);

            return appUser;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string userId)
        {
            var appUser = await userManager.FindByIdAsync(userId);

            return appUser;
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure = false)
        {
            var result = await signInManager.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);

            return result;
        }


        public async Task RegisterUserAsync(BitrixUser bitrixUser, string password)
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

        public async Task<ApplicationUser?> GetCurrentUser()
        {
            var userId = await GetUserIdAsync();

            if (string.IsNullOrEmpty(userId))
                return null;

            var currentUser = await FindByIdAsync(userId);

            return currentUser;
        }
    }
}
