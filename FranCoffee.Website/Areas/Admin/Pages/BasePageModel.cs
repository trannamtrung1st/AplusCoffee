using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.Areas.Admin.Pages
{
    public abstract class BasePageModel : PageModel
    {
        public abstract PageInfo Info { get; }
    }
}
