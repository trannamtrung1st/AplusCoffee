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
    public class ContactUsModel : BasePageModel<ContactUsModel>
    {
        public ContactUsModel(CoffeeContext context, 
            IHtmlLocalizer<ContactUsModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "contact",
            Title = "Liên hệ - Tư vấn",
            Description = "Thông tin liên hệ của chúng tôi A+Coffee"
        };

        public void OnGet()
        {
        }
    }
}