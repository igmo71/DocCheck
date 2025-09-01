using DocCheck.Data;
using DocCheck.Infrastructure.Bitrix;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DocCheck.Application
{
    public class AuthService(
       AuthenticationStateProvider authStateProvider,
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       IUserStore<ApplicationUser> userStore,
       BitrixService bitrixService,
       ILogger<AuthService> logger)
    {
        public async Task<string?> GetCurrentUserIdAsync()
        {
            var authState = await authStateProvider.GetAuthenticationStateAsync();

            var userId = authState?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return userId;
        }

        public async Task<ApplicationUser?> GetCurrentUser()
        {
            var userId = await GetCurrentUserIdAsync();

            if (string.IsNullOrEmpty(userId))
                return null;

            var currentUser = await FindByIdAsync(userId);

            return currentUser;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            var appUser = await userManager.FindByEmailAsync(email);

            return appUser;
        }

        public async Task<ApplicationUser?> FindByIdAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var appUser = await userManager.FindByIdAsync(userId);

            return appUser;
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure = false)
        {

            var bitrixUser = await bitrixService.GetUserAsync(userName, password);

            if (bitrixUser == null || string.IsNullOrEmpty(bitrixUser.EMAIL))
                return SignInResult.Failed;

            var appUser = await FindByEmailAsync(bitrixUser.EMAIL);

            if (appUser == null)
                await RegisterUserAsync(bitrixUser, password);

            if (appUser == null || appUser.UserName == null)
                return SignInResult.Failed;

            var result = await signInManager.PasswordSignInAsync(appUser.UserName, password, isPersistent, lockoutOnFailure);

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
                throw new InvalidOperationException("Failed to create a user");
            }

            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                result = await userManager.ConfirmEmailAsync(user, code);

                if (!result.Succeeded)
                    logger.LogError("{Source} {Operation} {@Errors}",
                        nameof(RegisterUserAsync), nameof(userManager.ConfirmEmailAsync), result.Errors);
            }

            await AddToRoleAsync(user, bitrixUser);

            await signInManager.SignInAsync(user, isPersistent: false);
        }

        private async Task AddToRoleAsync(ApplicationUser user, BitrixUser bitrixUser)
        {
            IdentityResult identityResult = bitrixUser.WORK_DEPARTMENT switch
            {
                "Отдел продаж" => await userManager.AddToRoleAsync(user, "Managers"),
                "Бухгалтерия" => await userManager.AddToRoleAsync(user, "Accounting"),
                _ => await userManager.AddToRoleAsync(user, "Operators"),
            };

            if (!identityResult.Succeeded)
            {
                logger.LogError("{Source} {Operation} {@Errors}",
                    nameof(RegisterUserAsync), nameof(userManager.CreateAsync), identityResult.Errors);
                throw new InvalidOperationException("Failed to add user to role");
            }
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

        public List<ApplicationUser> GetUsers()
        {
            var users = userManager.Users.ToList();

            return users;
        }

        public async Task<bool> IsUserInRole(ApplicationUser user, string role)
        {
            var isUserInRole = await userManager.IsInRoleAsync(user, role);
            var isUserAdmin = await userManager.IsInRoleAsync(user, "Admin");
            return isUserInRole || isUserAdmin;
        }
    }
}
