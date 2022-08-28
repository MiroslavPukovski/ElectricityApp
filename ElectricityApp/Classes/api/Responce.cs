namespace ElectricityApp.Classes.api
{
    public class Responce<T>
    {
        public T Content { get; init; }

        public ResponceStatus Status { get; init; }

        public static Responce<T> Success(T content)
        {
            return new Responce<T>
            {
                Content = content,
                Status = ResponceStatus.Success()
            };
        }

        public static Responce<T> Failure(T content, int code, string message)
        {
            return new Responce<T>
            {
                Content = content,
                Status = ResponceStatus.Failure(code, message)
            };
        }
    }

    public class Responce
    {

        public ResponceStatus Status { get; init; }

        public static Responce Success()
        {
            return new Responce
            {
                Status = ResponceStatus.Success()
            };
        }

        public static Responce Failure(int code, string message)
        {
            return new Responce
            {
                Status = ResponceStatus.Failure(code, message)
            };
        }
    }
}
