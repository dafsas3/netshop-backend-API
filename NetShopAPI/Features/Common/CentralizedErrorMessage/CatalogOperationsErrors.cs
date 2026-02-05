namespace NetShopAPI.Features.Common.CentralizedErrorMessage
{
    public static class CatalogOperationsErrors
    {

        public static readonly (string Code, string Message) CategoryNotFound =
            ("CATEGORY_NOT_FOUND", "Указанной категории не существует, ID: ");

        public static readonly (string Code, string Message) PositionAlreadyExists =
                ("POSITION_ALREADY_EXISTS", "Позиция с указанным названием уже существует, Name: ");

        public static readonly (string Code, string Message) DuplicateInRequest =
            ("DUPLICATE_IN_REQUEST", "Название дублируется в запросе, Name: ");
    }
}
