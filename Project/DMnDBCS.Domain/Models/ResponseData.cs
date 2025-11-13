namespace DMnDBCS.Domain.Models
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public string? ErrorMessage { get; set; }

        public static ResponseData<T> Success(T data)
        {
            return new ResponseData<T> { Data = data };
        }

        public static ResponseData<T> Error(string message, T? data = default)
        {
            return new ResponseData<T> { ErrorMessage = message, IsSuccessful = false, Data = data };
        }
    }
}
