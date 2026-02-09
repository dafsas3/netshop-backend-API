using FluentValidation;
using NetShopAPI.Features.Catalog.Categories.Command;

namespace NetShopAPI.Features.Catalog.Categories.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {

        public CreateCategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Название категории не может быть пустым!")
                .MinimumLength(5).WithMessage("Название категории должно содержать не менее 5 символов!")
                .MaximumLength(30).WithMessage("Название категории должно содержать не более 30 символов!");
        }

    }
}