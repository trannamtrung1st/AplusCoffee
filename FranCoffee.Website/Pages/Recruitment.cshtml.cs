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
    public class RecruitmentModel : BasePageModel<RecruitmentModel>
    {
        public RecruitmentModel(CoffeeContext context, 
            IHtmlLocalizer<RecruitmentModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "about-us",
            Title = "Cơ hội nghề nghiệp",
            Description = "Cơ hội tuyển dụng lúc nào cũng có với A+ Coffee"
        };

        public void OnGet()
        {
        }
    }
}