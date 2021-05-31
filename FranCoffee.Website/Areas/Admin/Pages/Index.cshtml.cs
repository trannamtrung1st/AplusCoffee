using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages
{
    public class IndexModel : BasePageModel
    {
        public override PageInfo Info => new PageInfo()
        {
            Menu = "",
            Title = "",
        };

        public IActionResult OnGet()
        {
            return LocalRedirect("/admin/blog");
        }
    }
}