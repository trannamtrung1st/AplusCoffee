using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.Pages
{
    public abstract class BasePageModel<T> : PageModel
    {
        public IHtmlLocalizer<T> L { get; set; }
        public IHtmlLocalizer<App> SharedL { get; set; }
        public abstract PageInfo Info { get; }
        protected CoffeeContext _context;

        public List<ProductTypeDetailViewModel> ProductTypes { get; set; }
        public BasePageModel(CoffeeContext context, IHtmlLocalizer<T> loc,
            IHtmlLocalizer<App> sharedL)
        {
            _context = context;
            ProductTypes = _context.ProductTypes.Select(p => new ProductTypeDetailViewModel
            {
                Id = p.Id,
                Name = p.Name.GetCurrentLangString()
            }).ToList();
            L = loc;
            SharedL = sharedL;
        }
    }
}
