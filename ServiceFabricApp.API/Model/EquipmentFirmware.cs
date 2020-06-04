using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceFabricApp.API.Model
{
    public class EquipmentFirmware
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("EquipmentId")]
        public string EquipmentId { get; set; }

        [JsonProperty("FirmwareDescription")]
        public string FirmwareDescription { get; set; }

        [JsonProperty("CurrentVersion")]
        public string CurrentVersion { get; set; }
    }
}
