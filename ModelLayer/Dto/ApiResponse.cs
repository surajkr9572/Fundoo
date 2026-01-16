using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Dto
{
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public string message { get; set; }
        public T Data { get; set; }
        public string[] Error { get; set; }

        //success Response Constructor
        public ApiResponse(bool success, string message, T data)
        {
            this.success = success;
            this.message = message;
            Data = data;
        }
        //Error Response Constructor
        public ApiResponse(bool success, string message, params string[] error)
        {
            this.success = success;
            this.message = message;
            Error = error;
        }
        //ParameterLess Constructor
        public ApiResponse() { }
    }
}
