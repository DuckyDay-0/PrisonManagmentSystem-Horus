using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS_Horus.Services
{
    public class ResultService<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
       public ResultService(bool success, string message = "", T? data = default) 
       {
            this.Success = success;
            this.Message = message;
            this.Data = data;
       }

        public override string ToString() => Message;
        
    }
}
