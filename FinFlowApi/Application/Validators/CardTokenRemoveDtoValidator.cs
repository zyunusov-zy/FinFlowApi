using FluentValidation;
using FinFlowApi.DTOs;

public class CardTokenRemoveDtoValidator : AbstractValidator<CardTokenRemoveAndUnblockDto>
{
    public CardTokenRemoveDtoValidator()
    {
        RuleFor(x => x.CardToken).NotEmpty().WithMessage("Card token required")
            .MinimumLength(32).WithMessage("Invalid Card token");
    }
}