using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Product
{
    public class IndexModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        public IndexModel(CoffeeContext context)
        {
            _context = context;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "product",
            Title = "Danh sách sản phẩm"
        };
        public List<ProductDetailViewModel> Products { get; set; }

        public string Message { get; set; }
        public string MessageType { get; set; }
        public void OnGet(string message = null, string mess_type = "success")
        {
            Products = _context.Products.OrderBy(c => c.ShowOrder).OrderBy(p => p.Name).ToList()
                .Select(p => new ProductDetailViewModel
                {
                    Id = p.Id,
                    Active = p.Active,
                    CategoryId = p.CategoryId,
                    Description = p.Description.GetCurrentLangString(),
                    Name = p.Name.GetCurrentLangString(),
                    CategoryName = p.Category?.Name.GetCurrentLangString(),
                    TypeName = p.Category?.Type.Name.GetCurrentLangString(),
                    ShowOrder = p.ShowOrder
                }).ToList();
            Message = message;
            MessageType = mess_type;
        }
    }
}