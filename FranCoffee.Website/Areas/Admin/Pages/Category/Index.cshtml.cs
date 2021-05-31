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
    public class IndexModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        public IndexModel(CoffeeContext context)
        {
            _context = context;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "category",
            Title = "Danh sách danh mục"
        };
        public IList<CategoryDetailViewModel> Categories { get; set; }

        public string Message { get; set; }
        public string MessageType { get; set; }
        public void OnGet(string message = null, string mess_type = "success")
        {
            Categories = _context.Categories.OrderBy(p => p.ShowOrder).ToList()
                .Select(c => new CategoryDetailViewModel
                {
                    Id = c.Id,
                    Name = c.Name.GetCurrentLangString(),
                    Description = c.Description.GetCurrentLangString(),
                    TypeId = c.TypeId,
                    TypeName = c.Type.Name.GetCurrentLangString(),
                    ShowOrder = c.ShowOrder
                }).ToList();
            Message = message;
            MessageType = mess_type;
        }
    }
}