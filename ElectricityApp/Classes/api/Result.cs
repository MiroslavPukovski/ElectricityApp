namespace ElectricityApp.Classes.api
{
    public class Result
    {
        public bool success { get; init; }
        public string error { get; init; }


        public Result()
        {
            success = true;
        }


        public Result(string message)
        {
            success = false;
            error = message;
        }

    }

    public class Result<T>
    {
        public bool success { get; init; }
        public string error { get; init; }
        public T Content { get; init; }


        public Result(T content)
        {
            success = true;
            Content = content;
        }


        public Result(string message, T content)
        {
            success = false;
            error = message;
            Content = content;
        }
    }
}
