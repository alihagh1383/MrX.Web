using MrX.Web.ApiCallback;

namespace MrX.Web.Exceptions
{
    public abstract class ApiException()
    {
        public Boolean IsSuccess { get; set; } = false;
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public Return ToReturn() => new(IsSuccess, StatusCode, Message, Data);
    }
}
