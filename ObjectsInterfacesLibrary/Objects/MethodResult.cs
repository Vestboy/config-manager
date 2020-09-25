using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsInterfacesLibrary.Objects
{
    public class MethodResult: IMethodResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

        public MethodResult(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public MethodResult(bool isSuccessful, string message, object result)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            Result = result;
        }
    }
}
