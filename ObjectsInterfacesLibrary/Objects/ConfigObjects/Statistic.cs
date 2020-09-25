using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectsInterfacesLibrary.Objects.ConfigObjects
{
    [JsonObject(ItemRequired = Required.Always)]
    public class Statistic
    {
        public int Health { get; set; }
        public int Vitality { get; set; }
    }
}
