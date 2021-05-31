using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class Branches
    {
        public Branches()
        {
            Posts = new HashSet<Posts>();
        }

        public int Id { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public string ImageUrl { get; set; }
        public string Area { get; set; }
        public string IntroContent { get; set; }
        public DateTime? OpeningDate { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }
    }
}
