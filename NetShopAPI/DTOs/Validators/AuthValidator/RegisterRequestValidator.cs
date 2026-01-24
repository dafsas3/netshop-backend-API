using FluentValidation;
using NetShopAPI.DTOs.AuthDTO;
using System.Text.RegularExpressions;
namespace NetShopAPI.DTOs.Validators.AuthValidator
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {

        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().MinimumLength(2).MaximumLength(100)
                .WithMessage("Имя должно содержать не менее 2 и не более 100 символов!");

            RuleFor(x => x.LastName)
                .NotEmpty().MinimumLength(2).MaximumLength(100)
                .WithMessage("Фамилия должна содержать не менее 2 и не более 100 символов!");

            RuleFor(x => x.SurName)
                .NotEmpty().MinimumLength(2).MaximumLength(100)
                .WithMessage("Отчество должно содержать не менее 2 и не более 100 символов!");

            RuleFor(x => x.NickName)
                .NotEmpty().MinimumLength(3).MaximumLength(30)
                .Matches(@"^[a-zA-Z0-9_.]+$")
                .WithMessage("Никнейм может содержать только латиницу, цифры, подчёркивание.");

            RuleFor(x => x.Email)
                .NotEmpty().MaximumLength(300)
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty().MaximumLength(30)
                .Must(BeValidPhone)
                .WithMessage("Номер телефона должен быть международного формата.");

            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(100)
                .WithMessage("Минимальная длина пароля: 8, максимальная: 100")
                .Matches(@"[A-Za-z]").WithMessage("Пароль должен содержать минимум 1 букву")
                .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Matches(@"^\S+$").WithMessage("Пароль не должен содержать пробелы");
        }


        private static bool BeValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            var digits = Regex.Replace(phone, @"\D", "");
            return Regex.IsMatch(digits, @"(0\d{9}|380\d{9}|80\d{9})$");
        }

    }
}
