using FluentValidation;
using Core.DTOs.Client;

namespace BLL.Validators;

public class ClientCreateValidator : AbstractValidator<ClientCreateDTO>
{
    public ClientCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).EmailAddress();
        RuleForEach(x => x.PhoneNumbers).Matches(@"^\+7\d{10}$");
    }
}