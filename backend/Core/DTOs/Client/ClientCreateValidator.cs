using FluentValidation;

namespace Core.DTOs.Client;
public class ClientCreateValidator : AbstractValidator<ClientCreateDTO>
{
    public ClientCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(2);
        RuleFor(x => x.Email).EmailAddress();
        RuleForEach(x => x.PhoneNumbers).Matches(@"^\+\d{11}$");
    }
}