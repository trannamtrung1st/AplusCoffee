using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class ProductTypes
    {
        public ProductTypes()
        {
            Categories = new HashSet<Categories>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Categories> Categories { get; set; }
    }
}
