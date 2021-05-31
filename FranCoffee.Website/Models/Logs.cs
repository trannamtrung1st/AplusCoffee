using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class Logs
    {
        public string Id { get; set; }
        public string JsonContent { get; set; }
        public DateTime? Time { get; set; }
    }
}
