using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.DAOs;

namespace FUNewsManagement_DataAccess.Repositories;

public class SystemAccountRepository : ISystemAccountRepository
{
    public SystemAccount? GetAccountByEmailAndPassword(string email, string password)
        => SystemAccountDAO.Instance.GetAccountByEmailAndPassword(email, password);

    public List<SystemAccount> GetAccounts(string? searchTerm = null)
        => SystemAccountDAO.Instance.GetAccounts(searchTerm);

    public SystemAccount? GetAccountById(short accountId)
        => SystemAccountDAO.Instance.GetAccountById(accountId);

    public void AddAccount(SystemAccount account)
        => SystemAccountDAO.Instance.AddAccount(account);

    public void UpdateAccount(SystemAccount account)
        => SystemAccountDAO.Instance.UpdateAccount(account);

    public void DeleteAccount(short accountId)
        => SystemAccountDAO.Instance.DeleteAccount(accountId);

    public short GetNextAccountId()
        => SystemAccountDAO.Instance.GetNextAccountId();
}
