using System.Collections.Generic;

namespace PaskolProd.Models.Response
{
    public class ActionResponse
    {
        public bool Succeeded { get; set; }
        public Dictionary<string, string[]> FiledErrors{ get; set; }
    }
} 