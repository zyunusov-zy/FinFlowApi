using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;


using FinFlowApi.DTOs;
using FinFlowApi.Services;
using FinFlowApi.Validators;

namespace FinFlowApi.Controllers;

[Route("api/v1")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly ICardService _cardService;
    private readonly IValidator<OtpDtoRequest> _otpDtoRequestValidator;
    private readonly IValidator<VerifyDtoRequest> _verifyDtoRequestValidator;


    public OtpController(ICardService cardService, IValidator<OtpDtoRequest> otpDtoRequestValidator, IValidator<VerifyDtoRequest> verifyDtoRequestValidator)
    {
        _cardService = cardService;
        _otpDtoRequestValidator = otpDtoRequestValidator;
        _verifyDtoRequestValidator = verifyDtoRequestValidator;
    }

    [HttpPost("otp")]
    [Authorize]
    public async Task<IActionResult> GetOtp([FromBody] OtpDtoRequest dto)
    {
        var validationResult = await _otpDtoRequestValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            });

            return BadRequest(errors);
        }

        try
        {
            var (otp, otpId) = await _cardService.CardVerification(dto);
            return Ok(new
            {
                result = new
                {
                    id = otpId,
                    otp = otp
                }
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("otp/verify")]
    [Authorize]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyDtoRequest dto)
    {
        var validationResult = await _verifyDtoRequestValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            });

            return BadRequest(errors);
        }


        try
        {
            var refNum = await _cardService.VerifyOtp(dto);
            return Ok(new
            {
                result = new
                {
                    refNum = refNum
                }
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
