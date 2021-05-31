using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Areas.Admin.Pages
{
    public class LoginModel : PageModel
    {

        private readonly IdentityContext _context;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        public LoginModel(IdentityContext context,
            UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Message { get; set; }
        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string message = null, string return_url = "/admin")
        {
            if (User.Identity.IsAuthenticated)
                return LocalRedirect(return_url);
            Message = message;
            ReturnUrl = return_url;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(LoginViewModel model,
            string return_url = "/admin")
        {
            if (_context.Users.Count() == 0)
            {
                var createResult = await _userManager.CreateAsync(new AppUsers
                {
                    UserName = model.username
                }, model.password);
                if (!createResult.Succeeded)
                    throw new Exception(JsonConvert.SerializeObject(createResult));
            }

            //Tìm user bằng username, password
            var signInResult = await
                _signInManager.PasswordSignInAsync(model.username, model.password, false, false);

            if (!signInResult.Succeeded)
                return LocalRedirect($"/admin/login?message=Sai thông tin đăng nhập" +
                    $"&return_url={HttpUtility.UrlEncode(return_url)}");
            return LocalRedirect(return_url);
        }
    }

}