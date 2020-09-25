using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ObjectsInterfacesLibrary.Objects.ConfigObjects
{
    [JsonObject(ItemRequired = Required.Always)]
    public class ConfigOne 
    {
        public string Dsn { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public List<string> Colors { get; set; }
        public Statistic Stat { get; set; }
    }
}
