using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Category
{
    public class AddModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        public AddModel(CoffeeContext context)
        {
            _context = context;
        }

        public override PageInfo Info => new PageInfo()
        {
            Title = "Thêm mới danh mục",
            Menu = "category"
        };

        public List<ProductTypeDetailViewModel> ProductTypes { get; set; }
        public void OnGet()
        {
            ProductTypes = _context.ProductTypes.ToList().Select(c => new ProductTypeDetailViewModel
            {
                Id = c.Id,
                Name = c.Name.GetCurrentLangString()
            }).ToList();
        }

        public IActionResult OnPost(AddCategoryViewModel model)
        {
            var cate = new Categories
            {
                Description = model.description.ToMultilangString().ToJson(),
                Name = model.name.ToMultilangString().ToJson(),
                TypeId = model.type_id,
                ShowOrder = model.show_order,
            };
            _context.Categories.Add(cate);
            _context.SaveChanges();

            return LocalRedirect("/admin/category?message=Thêm mới thành công");
        }
    }
}