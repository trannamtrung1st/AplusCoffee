using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{

    public class AdminProductDetailViewModel
    {
        public int Id { get; set; }
        public IDictionary<string, string> Name { get; set; }
        public IDictionary<string, string> Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? Active { get; set; }
        public IEnumerable<ProductImagesViewModel> Images { get; set; }
        public int? ShowOrder { get; set; }
    }

    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public bool? Active { get; set; }
        public IEnumerable<ProductImagesViewModel> Images { get; set; }
        public string CategoryName { get; set; }
        public string TypeName { get; set; }
        public int? ShowOrder { get; set; }
    }

    public class AddProductViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public int? category_id { get; set; }
        public bool? active { get; set; }
        public IFormFile avatar { get; set; }
        public IEnumerable<IFormFile> detail { get; set; }
        public int? show_order { get; set; }
    }

    public class UpdateProductViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public int? category_id { get; set; }
        public bool? active { get; set; }
        public IFormFile avatar { get; set; }
        public IEnumerable<IFormFile> detail { get; set; }
        public int? show_order { get; set; }
    }

    public class ProductImagesViewModel
    {
        public string RelPath { get; set; }
        public bool IsAvatar { get; set; }
        public int? ShowOrder { get; set; }
    }
}
