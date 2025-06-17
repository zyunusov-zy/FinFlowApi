using FluentValidation;
using FinFlowApi.DTOs;

namespace FinFlowApi.Validators;

public class VerifyDtoRequestValidator : AbstractValidator<VerifyDtoRequest>
{
    public VerifyDtoRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id required");
        RuleFor(x => x.Token).NotEmpty().WithMessage("Token required");
    }
}