namespace AcrylicWindow.Client.Core.Model
{
    public class AuthorizationResult<T>
        where T : class
    {
        private const string empty = "";

        public AuthorizationResult() : this(null, empty)
        {
        }

        public AuthorizationResult(T response, string errorMessage = empty)
        {
            Response = response;
            ErrorMessage = errorMessage;
        }

        public T Response { get; set; }

        public string ErrorMessage { get; set; }

        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    }
}
