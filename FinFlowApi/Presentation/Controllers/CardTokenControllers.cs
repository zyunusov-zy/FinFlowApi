using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;

using FinFlowApi.DTOs;
using FinFlowApi.Services;
using FinFlowApi.Validators;

namespace FinFlowApi.Controllers;

[ApiController]
[Route("api/v1/card_token")]
public class CardTokenController : ControllerBase
{
    private readonly ICardTokenService _cardTokenService;
    private readonly IValidator<CardTokenRemoveDto> _cardTokenRemoveValidator;
    private readonly IValidator<BlockCardTokenDto> _blockCardTokenValidator;

    public CardTokenController(
        ICardTokenService cardTokenService,
        IValidator<CardTokenRemoveDto> cardTokenRemoveValidator,
        IValidator<BlockCardTokenDto> blockCardTokenValidator)
    {
        _cardTokenRemoveValidator = cardTokenRemoveValidator;
        _cardTokenService = cardTokenService;
        _blockCardTokenValidator = blockCardTokenValidator;
    }

    [HttpPost("remove")]
    [Authorize]
    public async Task<IActionResult> RemoveCardToken([FromBody] CardTokenRemoveDto dto)
    {
        var validationResult = await _cardTokenRemoveValidator.ValidateAsync(dto);
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
            var status = await _cardTokenService.RemoveCardToken(dto);
            return Ok(new
            {
                result = new
                {
                    id = dto.CardToken,
                    status = status
                }
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                status = 1
            });
        }
    }

    [HttpPost("block")]
    [Authorize]
    public async Task<IActionResult> BlockCardToken([FromBody]  BlockCardTokenDto dto)
    {
        var validationResult = await _blockCardTokenValidator.ValidateAsync(dto);
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
            var status = await _cardTokenService.BlockCardTokenS(dto);
            return Ok(new
            {
                result = new
                {
                    id = dto.CardToken,
                    status = status
                }
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                status = 1
            });
        }
    }
}