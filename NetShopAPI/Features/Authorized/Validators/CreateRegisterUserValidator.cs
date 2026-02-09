using FluentValidation;
using NetShopAPI.Features.Authorized.Register.Commands;
using System.Text.RegularExpressions;

namespace NetShopAPI.Features.Authorized.Validators
{
    public class CreateRegisterUserValidator : AbstractValidator<CreateRegisterUserCommand>
    {

        public CreateRegisterUserValidator()
        {
            RuleFor(u => u.FirstName)
                .NotEmpty().WithMessage("Имя не может быть пустым!")
                .Length(2, 20).WithMessage("Минимальная длина имени: 2 символа. Максимальная длина: 20.");

            RuleFor(u => u.SurName)
                .NotEmpty().WithMessage("Фамилия не может быть пустой!")
                .Length(2, 30).WithMessage("Минимальная длина фамилии: 2 символа. Максимальная длина: 30 символов.");

            RuleFor(u => u.LastName)
                .MaximumLength(40).WithMessage("Максимальная длина отчества: 40 символов!");

            RuleFor(u => u.NickName)
                .NotEmpty().WithMessage("Никнейм не может быть пустым!")
                .Length(5, 20).WithMessage("Минимальная длина никнейма: 5 символов. Максимальная длина: 20 символов.")
                .Matches(@"^[a-zA-Z0-9_.]+$").WithMessage("Никнейм может содержать только латиницу, цифры и подчёркивания!");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Эмаил обязателен к заполнению!")
                .EmailAddress()
                .MaximumLength(320).WithMessage("Эмаил - адрес должен быть длиной не более 320 символов!");

            RuleFor(u => u.Phone)
               .Must(BeValidPhone)
               .When(u => !string.IsNullOrWhiteSpace(u.Phone))
               .WithMessage("Неверный формат телефона.");

            RuleFor(u => u.Password)
               .NotEmpty().WithMessage("Пароль обязателен к заполнению!")
               .MinimumLength(8).WithMessage("Пароль должен быть длиной не менее 8 символов!")
               .MaximumLength(150).WithMessage("Пароль должен быть длиной не более 150 символов!")
               .Matches(@"[A-Za-z]").WithMessage("Пароль должен содержать минимум 1 букву!")
               .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру!")
               .Matches(@"^\S+$").WithMessage("Пароль не должен содержать пробелы!");
        }


        private static bool BeValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return true;

            var digits = Regex.Replace(phone, @"\D", "");
            return digits.Length >= 10 && digits.Length <= 15;
        }

    }
}
