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
    public class FranchiseModel : BasePageModel<FranchiseModel>
    {
        public FranchiseModel(CoffeeContext context, 
            IHtmlLocalizer<FranchiseModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "franchise",
            Title = "Nhượng quyền",
            Description = "Nhượng quyền là cách tốt nhất để cùng nhau phát triển, hay tham gia cùng chúng tôi"
        };

        public void OnGet()
        {
        }
    }
}