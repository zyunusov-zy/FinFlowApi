using FluentValidation;
using FinFlowApi.DTOs;

namespace FinFlowApi.Validators;

public class OtpDtoRequestValidator : AbstractValidator<OtpDtoRequest>
{
    public OtpDtoRequestValidator()
    {
        RuleFor(x => x.Pan).NotEmpty().WithMessage("Pan field required")
            .MinimumLength(16).WithMessage("Invalid card credentials");
        RuleFor(x => x.Expiry).NotEmpty().WithMessage("Expiry required")
            .MinimumLength(4).WithMessage("Invalid card credentials")
            .MaximumLength(4).WithMessage("Invalid card credentials");
    }
}