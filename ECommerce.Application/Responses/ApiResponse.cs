using ECommerce.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Responses
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public ResponseStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new() { Success = true, StatusCode = ResponseStatusCode.OK, Message = message ?? "Success", Data = data };

        public static ApiResponse<T> Created(T data, string? message = null) =>
            new() { Success = true, StatusCode = ResponseStatusCode.Created, Message = message ?? "Created", Data = data };

        public static ApiResponse<T> Fail(ResponseStatusCode statusCode, string message, List<string>? errors = null) =>
            new() { Success = false, StatusCode = statusCode, Message = message, Errors = errors };
    }
}
