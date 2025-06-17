using Oracle.ManagedDataAccess.Client;

namespace FinFlowApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<bool> UserExistsAsync(string username, string password)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var query = "SELECT COUNT(*) FROM Users WHERE username = :username AND password = :password";
        using var command = new OracleCommand(query, connection);

        command.Parameters.Add(new OracleParameter("username", username));
        command.Parameters.Add(new OracleParameter("password", password));

        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result) > 0;
    }
}
