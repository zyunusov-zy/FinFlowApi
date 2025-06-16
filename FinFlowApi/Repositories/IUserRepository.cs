namespace FinFlowApi.Repositories;
public interface IUserRepository
{
    Task<bool> UserExistsAsync(string username, string password);
}
