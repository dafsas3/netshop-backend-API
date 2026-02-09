using FluentValidation;
using NetShopAPI.Features.Catalog.Positions.DTOs.Request;
using NetShopAPI.Features.Catalog.Validators;

namespace NetShopAPI.Features.Catalog.Positions.Validators
{
    public class BulkCreatePositionRequestValidator : AbstractValidator<BulkCreatePositionRequest>
    {

        public BulkCreatePositionRequestValidator()
        {
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Данные о позициях обязательны.")
                .NotEmpty().WithMessage("Данные о позициях не могут быть пустыми")
                .Must(items => items.Count <= 500).WithMessage("Максимальное количество создаваемых позиций за раз: 500.");

            RuleForEach(x => x.Items).SetValidator(new BulkCreatePositionsValidator());
        }

    }
}
