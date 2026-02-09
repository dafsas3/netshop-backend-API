using FluentValidation;
using NetShopAPI.Features.Authorized.Login.Query;
using System.Text.RegularExpressions;

namespace NetShopAPI.Features.Authorized.Validators
{
    public class AuthorizedUserValidator : AbstractValidator<AuthorizationUserCommand>
    {

        public AuthorizedUserValidator()
        {
            RuleFor(u => u)
                .Must(u => !string.IsNullOrWhiteSpace(u.Email) || !string.IsNullOrWhiteSpace(u.Phone))
                .WithMessage("Укажите email или номер телефона!");

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Неккоретный email формат!")
                .MaximumLength(320).WithMessage("Максимальная длина email: 320 символов!")
                .When(u => !string.IsNullOrWhiteSpace(u.Email));    

            RuleFor(u => u.Phone)
                .Must(BeValidPhone).WithMessage("Номер телефона должен быть международного формата.")
                .When(u => !string.IsNullOrWhiteSpace(u.Phone));

            RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Пароль обязателен к заполнению!");
        }


        private static bool BeValidPhone(string phone)
        {
            var digits = Regex.Replace(phone, @"\D", "");
            return digits.Length is >= 10 and <= 15;
        }

    }
}
