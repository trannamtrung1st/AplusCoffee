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
    public class VisionMissionModel : BasePageModel<VisionMissionModel>
    {
        public VisionMissionModel(CoffeeContext context,
            IHtmlLocalizer<VisionMissionModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo
        {
            Menu = "about-us",
            Title = "Tầm nhìn & Sứ mệnh",
            Description = "Tầm nhìn và sứ mệnh của chúng tôi, A+Coffee"
        };

        public void OnGet()
        {
        }
    }
}