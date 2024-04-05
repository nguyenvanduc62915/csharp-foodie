using AppCore.Models;
using AppCore.Models.ViewModels;
namespace AppCore.App.Repositories;
public interface IAccountManagerRepository
{
    Task<IEnumerable<AccountViewModel>> GetAllAccountsAsync();
    Task<Account> GetAccountByIdAsync(string accountId);
    Task<bool> CreateAccountAsync(Account account, string password);
    Task<bool> UpdateAccountAsync(Account account);
    Task<bool> DeleteAccountAsync(string accountId);
    Task<IEnumerable<string>> GetRolesAsync(Account account);
    Task<bool> AddToRoleAsync(Account account, string role);
    Task<bool> RemoveFromRoleAsync(Account account, string role);
}   