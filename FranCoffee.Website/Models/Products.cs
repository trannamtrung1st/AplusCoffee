using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class Products
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public bool? Active { get; set; }
        public int? CategoryId { get; set; }
        public int? ShowOrder { get; set; }

        public virtual Categories Category { get; set; }
    }
}
