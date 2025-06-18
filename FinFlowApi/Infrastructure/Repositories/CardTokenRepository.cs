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

    public async Task<int> RemoveCardTokenAsync(CardTokenRemoveDto dto)
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

}