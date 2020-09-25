using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectsInterfacesLibrary.Objects.ConfigObjects
{
    [JsonObject(ItemRequired = Required.Always)]
    public class ServiceEnvironment
    {
        public string Name { get; set; }
        public List<string> Services { get; set; }
    }
}
