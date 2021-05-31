using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class AddCategoryViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public int? type_id { get; set; }
        public int? show_order { get; set; }
    }

    public class UpdateCategoryViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public int? type_id { get; set; }
        public int? show_order { get; set; }
    }

    public class AdminCategoryDetailViewModel
    {
        public int Id { get; set; }
        public IDictionary<string, string> Name { get; set; }
        public IDictionary<string, string> Description { get; set; }
        public int? TypeId { get; set; }
        public int? ShowOrder { get; set; }
    }

    public class CategoryDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public int? ShowOrder { get; set; }
    }
}
