using FinFlowApi.DTOs;
using Oracle.ManagedDataAccess.Client;
using System.Data;

using FinFlowApi.Services;
namespace FinFlowApi.Repositories;

public class CardTokenRepository : ICardTokenRepository
{
    private readonly string _connectionString;

    public CardTokenRepository(string connection)
    {
        _connectionString = connection;
    }

    public async Task<int> RemoveCardTokenAsync(CardTokenRemoveAndUnblockDto dto)
    {

        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var updateQuery = @"UPDATE Cards SET RefNum = NULL WHERE RefNum = :Token";
        using var updateCommand = new OracleCommand(updateQuery, connection);
        updateCommand.Parameters.Add(new OracleParameter("Token", dto.CardToken));

        var affected = await updateCommand.ExecuteNonQueryAsync();

        return affected > 0 ? 0 : -1;
    }

    public async Task<int> BlockCardTokenAsync(BlockCardTokenDto dto)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var expiresAt = DateTime.UtcNow.AddMinutes(dto.TimeBlock);

        var updateQuery = @"UPDATE Cards SET BlockTime = :timeBlock WHERE RefNum = :Token";
        using (var updateCommand = new OracleCommand(updateQuery, connection))
        {
            updateCommand.Parameters.Add(new OracleParameter("timeBlock", OracleDbType.TimeStamp)
            {
                Value = expiresAt
            });
            updateCommand.Parameters.Add(new OracleParameter("Token", dto.CardToken));

            var affected = await updateCommand.ExecuteNonQueryAsync();
            return affected > 0 ? 0 : -1;
        }
    }

    public async Task<int> UnblockCardTokenAsync(CardTokenRemoveAndUnblockDto dto)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();


        var updateQuery = @"UPDATE Cards SET BlockTime = NULL WHERE RefNum = :Token";
        using (var updateCommand = new OracleCommand(updateQuery, connection))
        {
            updateCommand.Parameters.Add(new OracleParameter("Token", dto.CardToken));

            var affected = await updateCommand.ExecuteNonQueryAsync();
            return affected > 0 ? 0 : -1;
        }
    }

    public async Task<(string?, decimal)> CardBalanceCheckAsync(CardTokenRemoveAndUnblockDto dto)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var getQuery = @"SELECT Pan, Balance FROM Cards WHERE RefNum = :Token";
        using (var getCommand = new OracleCommand(getQuery, connection))
        {
            getCommand.Parameters.Add(new OracleParameter("Token", dto.CardToken));
            using (var reader = await getCommand.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    string pan = reader["Pan"]?.ToString()!;
                    decimal balance = reader["Balance"] != DBNull.Value
                        ? Convert.ToDecimal(reader["Balance"])
                        : 0;

                    return (pan, balance);
                }
                else
                {
                    return (null, -1);
                }
            }
        }

    }

}