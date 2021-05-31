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
    public class CoreValueModel : BasePageModel<CoreValueModel>
    {
        public CoreValueModel(CoffeeContext context, 
            IHtmlLocalizer<CoreValueModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "about-us",
            Title = "Giá trị cốt lõi",
            Description = "Đâu là giá trị cốt lõi của A+Coffee, hãy đọc thêm chi tiết"
        };

        public void OnGet()
        {
        }
    }
}