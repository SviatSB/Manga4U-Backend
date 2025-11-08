namespace Domain.Results
{
    public class ProxyResult
    {
        private ProxyResult() { }

        public static ProxyResult Success(string result) =>
            new()
            {
                IsSucceed = true,
                ErrorMessage = null,
                Result = result
            };

        public static ProxyResult Failure(string error) =>
            new()
            {
                IsSucceed = false,
                ErrorMessage = error,
                Result = null
            };

        public bool IsSucceed { get; private set; }
        public string? ErrorMessage { get; private set; }
        public string? Result { get; private set; }
    }
}
