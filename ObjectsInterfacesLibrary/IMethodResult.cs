using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsInterfacesLibrary
{
    public interface IMethodResult
    {
        bool IsSuccessful { get; set; }
        string Message { get; set; }
        object Result { get; set; }
    }
}
