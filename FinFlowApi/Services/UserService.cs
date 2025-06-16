using FinFlowApi.Repositories;

namespace FinFlowApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;

    public UserService(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public async Task<bool> CheckUserExists(string username, string password)
    {
        return await _userRepo.UserExistsAsync(username, password);
    }
}
