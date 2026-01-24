using FluentValidation;
using NetShopAPI.DTOs.AuthDTO;

namespace NetShopAPI.DTOs.Validators.AuthValidator
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Login)
                .MinimumLength(2).MaximumLength(300)
                .WithMessage("Должен содержать номер телефона или почту, привязанную к аккаунту."+
                " Корректный диапозон: 2-300 (включительно)!");
                
            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(8).MaximumLength(100)
                .WithMessage("Минимальная длина пароля: 8, максимальная: 100")
                .Matches(@"[A-Za-z]").WithMessage("Пароль должен содержать минимум 1 букву")
                .Matches(@"\d").WithMessage("Пароль должен содержать хотя бы одну цифру")
                .Matches(@"^\S+$").WithMessage("Пароль не должен содержать пробелы");
        }
    }
}
