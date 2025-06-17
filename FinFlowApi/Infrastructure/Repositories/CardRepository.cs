using FinFlowApi.DTOs;
using Oracle.ManagedDataAccess.Client;
using FinFlowApi.Services;
using System.Data;

namespace FinFlowApi.Repositories;

public class CardRepository : ICardRepository
{

    private readonly string _connectionString;

    public CardRepository(string connection)
    {
        _connectionString = connection;
    }
    public async Task<(string?, string?)> VerifyCardAsync(OtpDtoRequest dto)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var checkQuery = "SELECT COUNT(*) FROM Cards WHERE pan = :pan AND expirydate = :expiry";
        using (var checkCommand = new OracleCommand(checkQuery, connection))
        {
            checkCommand.Parameters.Add(new OracleParameter("pan", dto.Pan));
            checkCommand.Parameters.Add(new OracleParameter("expiry", dto.Expiry));

            var result = await checkCommand.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            if (count == 0)
                return (null, null);
        }

        string otp = OtpService.GenerateOtp();

        var updateQuery = @"
            UPDATE Cards 
            SET otp = :otp, otpid = otp_id_seq.NEXTVAL 
            WHERE pan = :pan AND expirydate = :expiry 
            RETURNING otpid INTO :otpId";

        using (var updateCommand = new OracleCommand(updateQuery, connection))
        {
            updateCommand.Parameters.Add(new OracleParameter("otp", OracleDbType.Varchar2)
            {
                Value = otp
            });

            updateCommand.Parameters.Add(new OracleParameter("pan", dto.Pan));
            updateCommand.Parameters.Add(new OracleParameter("expiry", dto.Expiry));

            var otpIdParam = new OracleParameter("otpId", OracleDbType.Int64)
            {
                Direction = ParameterDirection.Output
            };
            updateCommand.Parameters.Add(otpIdParam);

            await updateCommand.ExecuteNonQueryAsync();

            string? otpId = otpIdParam.Value.ToString();
            return (otp, otpId);
        }
    }
    public async Task<int> VerifyOtpAsync(VerifyDtoRequest dto)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var checkQuery = "SELECT COUNT(*) FROM Cards WHERE otpid = :otpid AND otp = :otp";
        using (var checkCommand = new OracleCommand(checkQuery, connection))
        {
            checkCommand.Parameters.Add(new OracleParameter("otpid", dto.Id));
            checkCommand.Parameters.Add(new OracleParameter("otp", dto.Token));

            var result = await checkCommand.ExecuteScalarAsync();
            var count = Convert.ToInt32(result);

            return count > 0 ? 0 : -1;
        }
    }

    public async Task SaveRefNumAsync(VerifyDtoRequest dto, string refNum)
    {
        using var connection = new OracleConnection(_connectionString);
        await connection.OpenAsync();

        var sql = @"UPDATE Cards
                    SET RefNum = :RefNum
                    WHERE otpid = :OtpId AND otp = :Otp";

        using var command = new OracleCommand(sql, connection);
        command.Parameters.Add(new OracleParameter("RefNum", refNum));
        command.Parameters.Add(new OracleParameter("OtpId", dto.Id));
        command.Parameters.Add(new OracleParameter("Otp", dto.Token));

        var rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected == 0)
            throw new Exception("Failed to update refNum: OTP not found or already used.");
    }
}