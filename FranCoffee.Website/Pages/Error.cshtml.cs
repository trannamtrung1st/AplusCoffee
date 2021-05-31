using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Pages
{
    public class ErrorModel : BasePageModel<ErrorModel>
    {

        public ErrorModel(CoffeeContext context,
            IHtmlLocalizer<ErrorModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo
        {
            Menu = null,
            Title = "Error",
            Description = "Có lỗi xảy ra"
        };

        public void OnGet()
        {
            var exceptionHandlerPathFeature =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature?.Error != null)
            {
                _context.Logs.Add(new Logs
                {
                    Id = Guid.NewGuid().ToString(),
                    JsonContent = JsonConvert.SerializeObject(exceptionHandlerPathFeature.Error),
                    Time = DateTime.UtcNow
                });
                _context.SaveChanges();
            }
        }
    }
}