using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ObjectsInterfacesLibrary.Objects.ConfigObjects
{
    [JsonObject(ItemRequired = Required.Always)]
    public class ConfigTwo
    {
        public string Name { get; set; }
        public List<ServiceEnvironment> Environments { get; set; }
        
    }
}
