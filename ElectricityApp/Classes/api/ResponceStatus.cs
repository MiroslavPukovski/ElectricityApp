namespace ElectricityApp.Classes.api
{
    public class ResponceStatus
    {
        public bool isSuccess { get; init; }
        public int StatusCode { get; init; }
        public string ErrorMessage { get; init; }

        public static ResponceStatus Success()
        {
            return new ResponceStatus
            {
                isSuccess = true,
                StatusCode = 200,
                ErrorMessage = string.Empty
            };
        }

        public static ResponceStatus Failure(int code, string message)
        {
            return new ResponceStatus
            {
                isSuccess = false,
                StatusCode = code,
                ErrorMessage = message
            };
        }
    }
}
