using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Pages
{
    public class AboutUsModel : BasePageModel<AboutUsModel>
    {
        public AboutUsModel(CoffeeContext context,
            IHtmlLocalizer<AboutUsModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "about-us",
            Title = "Giới thiệu chung",
            Description = "Thông tin về chúng tôi, A+ Coffee"
        };

        public int BranchCount { get; set; }

        public void OnGet()
        {
            BranchCount = _context.Branches.Where(b => b.Active == true).Count();
        }
    }
}