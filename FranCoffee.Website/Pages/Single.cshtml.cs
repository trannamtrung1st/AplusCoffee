using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Pages
{
    public class SingleModel : BasePageModel<SingleModel>
    {

        public SingleModel(CoffeeContext context,
            IHtmlLocalizer<SingleModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "product-menu",
            Title = "Thông tin sản phẩm",
            Description = Entity.Description?.ToMultilangString()["vi"]
        };

        private Products Entity { get; set; }
        public ProductDetailViewModel Product { get; set; }
        public List<ProductDetailViewModel> RelatedProducts { get; set; }
        public ProductTypeDetailViewModel ProductType { get; set; }

        public IActionResult OnGet(int id)
        {
            Entity = _context.Products.FirstOrDefault(b => b.Id == id && b.Active == true);
            if (Entity == null)
                return LocalRedirect("/san-pham");
            var type = Entity.Category.Type;
            ProductType = new ProductTypeDetailViewModel
            {
                Id = type.Id,
                Name = type.Name.GetCurrentLangString()
            };
            Product = new ProductDetailViewModel
            {
                Active = Entity.Active,
                CategoryId = Entity.CategoryId,
                CategoryName = Entity.Category.Name.GetCurrentLangString(),
                Name = Entity.Name.GetCurrentLangString(),
                Description = Entity.Description.GetCurrentLangString(),
                Id = Entity.Id,
                Images = Entity.Images != null ?
                    JsonConvert.DeserializeObject<IEnumerable<ProductImagesViewModel>>(Entity.Images) : null,
                TypeName = Entity.Category?.Type.Name.GetCurrentLangString()
            };

            var pro = _context.Products.OrderBy(c => c.ShowOrder)
                .OrderBy(c => c.Name).Where(p => p.Active == true && p.Id != Product.Id
                && p.CategoryId == Product.CategoryId)
                .Select(p => p.Id).ToList();
            var random = new Random();
            random.Shuffle(pro);
            pro = pro.Take(4).ToList();

            RelatedProducts = _context.Products.OrderBy(c => c.ShowOrder)
                .OrderBy(c => c.Name).Where(p => pro.Contains(p.Id)).ToList()
                .Select(p => new ProductDetailViewModel
                {
                    Active = p.Active,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name.GetCurrentLangString(),
                    Name = p.Name.GetCurrentLangString(),
                    Description = p.Description.GetCurrentLangString(),
                    Id = p.Id,
                    Images = p.Images != null ?
                    JsonConvert.DeserializeObject<IEnumerable<ProductImagesViewModel>>(p.Images) : null,
                    TypeName = p.Category?.Type.Name.GetCurrentLangString()
                }).ToList();

            return Page();
        }
    }
}