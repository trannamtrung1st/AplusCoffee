using System;
using System.Collections.Generic;

namespace FranCoffee.Website.Models
{
    public partial class Posts
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string Tags { get; set; }
        public string PostContent { get; set; }
        public string Author { get; set; }
        public string ThumbnailUrl { get; set; }
        public string PostDescription { get; set; }
        public int? BranchId { get; set; }
        public bool? Visivle { get; set; }

        public virtual Branches Branch { get; set; }
    }
}
