using FluentValidation;
using FinFlowApi.DTOs;

public class CardTokenRemoveDtoValidator : AbstractValidator<CardTokenRemoveDto>
{
    public CardTokenRemoveDtoValidator()
    {
        RuleFor(x => x.CardToken).NotEmpty().WithMessage("Card token required")
            .MinimumLength(32).WithMessage("Invalid Card token");
    }
}