using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.Repositories;

namespace FUNewsManagement_Services.Services;

public interface ISystemAccountService
{
    SystemAccount? Authenticate(string email, string password);
    List<SystemAccount> GetAccounts(string? searchTerm = null);
    SystemAccount? GetAccountById(short accountId);
    void AddAccount(SystemAccount account);
    void UpdateAccount(SystemAccount account);
    void DeleteAccount(short accountId);
    short GetNextAccountId();
}

public class SystemAccountService : ISystemAccountService
{
    private readonly ISystemAccountRepository _repository;

    public SystemAccountService(ISystemAccountRepository repository)
    {
        _repository = repository;
    }

    public SystemAccount? Authenticate(string email, string password)
        => _repository.GetAccountByEmailAndPassword(email, password);

    public List<SystemAccount> GetAccounts(string? searchTerm = null)
        => _repository.GetAccounts(searchTerm);

    public SystemAccount? GetAccountById(short accountId)
        => _repository.GetAccountById(accountId);

    public void AddAccount(SystemAccount account)
        => _repository.AddAccount(account);

    public void UpdateAccount(SystemAccount account)
        => _repository.UpdateAccount(account);

    public void DeleteAccount(short accountId)
        => _repository.DeleteAccount(accountId);

    public short GetNextAccountId()
        => _repository.GetNextAccountId();
}
