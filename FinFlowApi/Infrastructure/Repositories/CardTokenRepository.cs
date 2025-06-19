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

    public async Task<Dictionary<string, string>> GetCardInfoAsync(CardTokenRemoveAndUnblockDto dto,string username)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            SELECT 
                c.Pan, c.Balance, c.PhoneNumber, c.ExpiryDate, c.RefNum,
                h.FullName
            FROM Cards c
            JOIN CardHolder h ON c.HolderId = h.HolderId
            WHERE c.RefNum = :Token";

        using var command = new OracleCommand(query, connection);
        command.Parameters.Add(new OracleParameter("Token", dto.CardToken));

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var result = new Dictionary<string, string>
            {
                ["Id"] = reader["RefNum"]?.ToString() ?? "",
                ["Username"] = username,
                ["Pan"] = reader["Pan"]?.ToString() ?? "",
                ["Balance"] = reader["Balance"]?.ToString() ?? "0",
                ["PhoneNumber"] = reader["PhoneNumber"]?.ToString() ?? "",
                ["ExpiryDate"] = reader["ExpiryDate"]?.ToString() ?? "",
                ["FullName"] = reader["FullName"]?.ToString() ?? "",
                
            };

            return result;
        }

        throw new KeyNotFoundException("Car not found!!!");
    }


}