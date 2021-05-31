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
    public class MenuModel : BasePageModel<MenuModel>
    {
        public MenuModel(CoffeeContext context,
            IHtmlLocalizer<MenuModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "product-menu",
            Title = "Menu",
            Description = $"Mời đến với {TypeEntity.Name?.ToMultilangString()["vi"]} tuyệt vời của chúng tôi",
        };

        private ProductTypes TypeEntity { get; set; }

        public List<CategoryDetailViewModel> Categories { get; set; }
        public List<ProductDetailViewModel> Products { get; set; }
        public ProductTypeDetailViewModel ProductType { get; set; }

        public void OnGet(int type = 1)
        {
            TypeEntity = _context.ProductTypes.FirstOrDefault(p => p.Id == type);
            ProductType = new ProductTypeDetailViewModel
            {
                Id = TypeEntity.Id,
                Name = TypeEntity.Name.GetCurrentLangString()
            };
            Categories = _context.Categories.OrderBy(c => c.ShowOrder).Where(c => c.TypeId == type).ToList()
                .Select(c => new CategoryDetailViewModel
                {
                    Id = c.Id,
                    Description = c.Description.GetCurrentLangString(),
                    Name = c.Name.GetCurrentLangString(),
                    TypeId = c.TypeId
                }).ToList();
            Products = _context.Products.OrderBy(c => c.ShowOrder).OrderBy(c => c.Name)
                .Where(p => p.Active == true && p.Category.TypeId == type).ToList()
                .Select(p => new ProductDetailViewModel
                {
                    Active = p.Active,
                    CategoryId = p.CategoryId,
                    Description = p.Description.GetCurrentLangString(),
                    Id = p.Id,
                    Name = p.Name.GetCurrentLangString(),
                    Images = p.Images != null ?
                            JsonConvert.DeserializeObject<IEnumerable<ProductImagesViewModel>>(p.Images) : null
                }).ToList();
        }
    }
}