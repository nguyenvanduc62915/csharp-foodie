using AppCore.Models;
using AppCore.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppCore.App.Repositories;

public class AccountManagerRepository : IAccountManagerRepository
{
    private readonly UserManager<Account> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountManagerRepository(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public async Task<bool> AddToRoleAsync(Account account, string role)
    {
        var result = await _userManager.AddToRoleAsync(account, role);
        return result.Succeeded;
    }

    public async Task<bool> CreateAccountAsync(Account account, string password)
    {
        var result = await _userManager.CreateAsync(account, password);
        return result.Succeeded;
    }

    public async Task<bool> DeleteAccountAsync(string accountId)
    {
        var account = await _userManager.FindByIdAsync(accountId);

        if (account != null)
        {
            var result = await _userManager.DeleteAsync(account);
            return result.Succeeded;
        }

        return false;
    }

    public async Task<Account> GetAccountByIdAsync(string accountId)
    {
        return await _userManager.FindByIdAsync(accountId);
    }

    public async Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync()
    {
        var accounts = await _userManager.Users.ToListAsync();
        var accountViewModels = new List<AccountViewModel>();

        foreach (var account in accounts)
        {
            var roles = await _userManager.GetRolesAsync(account);
            var accountViewModel = new AccountViewModel
            {
                Id = account.Id,
                UserName = account.UserName,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Roles = roles.ToList()
            };

            accountViewModels.Add(accountViewModel);
        }

        return accountViewModels;
    }


    public async  Task<IEnumerable<string>> GetRolesAsync(Account account)
    {
        return await _userManager.GetRolesAsync(account);
    }

    public async Task<bool> RemoveFromRoleAsync(Account account, string role)
    {
        var result = await _userManager.RemoveFromRoleAsync(account, role);
        return result.Succeeded;
    }

    public async Task<bool> UpdateAccountAsync(Account account)
    {
        var existingAccount = await _userManager.FindByIdAsync(account.Id);

        if (existingAccount != null)
        {
            _userManager.UpdateAsync(existingAccount);
            return true;
        }

        return false;
    }
}
