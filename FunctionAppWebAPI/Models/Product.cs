using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FunctionAppWebAPI.Models
{
    public class Product
    {
        //[JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        
        //[JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }

        //[JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class ProductCreateModel
    {
        public string Name { get; set; }
    }

    public class ProductUpdateModel
    {
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
    }
}
