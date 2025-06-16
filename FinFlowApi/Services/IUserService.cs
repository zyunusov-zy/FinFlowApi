namespace FinFlowApi.Services;


public interface IUserService
{
    Task<bool> CheckUserExists(string username, string password);
}