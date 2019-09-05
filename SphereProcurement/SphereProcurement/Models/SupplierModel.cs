using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SphereProcurement.Models
{
    public class SupplierModel
    {
        [JsonProperty("supplierId")]
        public string supplierId {get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("address")]
        public string address { get; set; }

        [JsonProperty("contactNo")]
        public string contactNo { get; set; }

        [JsonProperty("email")]
        public string email { get; set; }

    }
}