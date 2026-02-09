namespace NetShopAPI.Features.Authorized.Common.CentralizedErrorMessage
{
    public static class AuthorizedOperationsErrors
    {
        public static readonly (string Code, string Message) NicknameAlreadyAxists =
            ("NICKNAME_ALREADY_EXISTS", "Данный никнейм уже существует.");

        public static readonly (string Code, string Message) PhoneOrEmailAlreadyExists =
            ("PHONE_OR_EMAIL_ALREADY_EXISTS", "Данный номер телефона или эмаил уже существует.");

        public static readonly (string Code, string Message) InvalidCredentials =
            ("INVALID_CREDENTIALS", "Неверный логин или пароль!");
    }
}
