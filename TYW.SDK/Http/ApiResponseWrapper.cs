namespace TYW.SDK.Http
{
    public class ApiResponseWrapper<T>
    {
        public T Data;

        public ApiErrorDetail Status { get; set; }
    }
}
