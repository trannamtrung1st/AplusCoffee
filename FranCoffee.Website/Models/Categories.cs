using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public int? ShowOrder { get; set; }

        public virtual ProductTypes Type { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
