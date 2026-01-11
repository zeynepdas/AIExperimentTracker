namespace Net9RestApi.DTOs
{
    //Tüm API response'ları için standart yapı
    public class ApiResponse<T>
    {
        public bool Success{ get; set; }
        public string Message{ get; set; } = string.Empty;
        public T? Data{ get; set; } 
        public static ApiResponse<T> SuccessResponse(T data, string message = "")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

    }
}