using FluentValidation;
using FinFlowApi.DTOs;

public class BlockCardTokenDtoValidator : AbstractValidator<BlockCardTokenDto>
{
    public BlockCardTokenDtoValidator()
    {
        RuleFor(x => x.CardToken).NotEmpty().WithMessage("Card token required")
            .MinimumLength(32).WithMessage("Invalid Card token");
        RuleFor(x => x.CardToken).NotEmpty().WithMessage("Time required in minutes");
    }
}