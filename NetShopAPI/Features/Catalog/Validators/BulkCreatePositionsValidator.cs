using FluentValidation;
using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;

namespace NetShopAPI.Features.Catalog.Validators
{
    public class BulkCreatePositionsValidator : AbstractValidator<BulkCreatePositionsCommand>
    {
        public BulkCreatePositionsValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название не может быть пустым!")
                .MinimumLength(2).WithMessage("Минимальная длина названия: 2 символа!")
                .MaximumLength(50).WithMessage("Максимальная длина названия: 50 символов!");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Стоимость обязательна!")
                .GreaterThan(0).WithMessage("Стоимость должна быть выше 0!")
                .LessThan(300000).WithMessage("Стоимость не может превышать 300.000!");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Количество не может быть пустым!")
                .GreaterThanOrEqualTo(0).WithMessage("Количество товара не может быть отрицательным!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Описание товара не может быть пустым!")
                .MinimumLength(20).WithMessage("Минимальная длина описания: 20 символов!")
                .MaximumLength(5000).WithMessage("Максимальная длина описания: 5.000 символов!");
        }
    }
}
