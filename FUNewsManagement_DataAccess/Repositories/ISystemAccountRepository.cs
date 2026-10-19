using FUNewsManagement_BusinessObjects.Entities;

namespace FUNewsManagement_DataAccess.Repositories;

public interface ISystemAccountRepository
{
    SystemAccount? GetAccountByEmailAndPassword(string email, string password);
    List<SystemAccount> GetAccounts(string? searchTerm = null);
    SystemAccount? GetAccountById(short accountId);
    void AddAccount(SystemAccount account);
    void UpdateAccount(SystemAccount account);
    void DeleteAccount(short accountId);
    short GetNextAccountId();
}
