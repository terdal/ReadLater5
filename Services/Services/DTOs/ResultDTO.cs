using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ResultDTO
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ResultDTO<T>
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }
}
