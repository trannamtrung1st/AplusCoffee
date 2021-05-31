using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Identity
{
    public class IndexModel : BasePageModel
    {
        private readonly UserManager<AppUsers> _userManager;
        public IndexModel(UserManager<AppUsers> userManager)
        {
            _userManager = userManager;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "identity",
            Title = "Chi tiết danh mục"
        };

        public string Message { get; set; }
        public string MessageType { get; set; }
        public string Username { get; set; }
        public IActionResult OnGet(string message = null, string mess_type = "success")
        {
            Message = message;
            MessageType = mess_type;

            Username = _userManager.GetUserName(User);
            return Page();
        }

        public async Task<IActionResult> OnPost(int id, UpdatePasswordViewModel model)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var checkPassResult = model.old_password != null &&
                await _userManager.CheckPasswordAsync(appUser, model.old_password);
            if (!checkPassResult)
                return LocalRedirect($"/admin/identity?message=Sai mật khẩu cũ&mess_type=error");
            if (model.new_password == null || model.confirm_password == null ||
                model.new_password.Length < 6 || model.confirm_password.Length < 6
                || !model.new_password.Equals(model.confirm_password))
                return LocalRedirect($"/admin/identity?message=Mật khẩu mới phải khớp với mật khẩu xác nhận và có độ dài lớn hơn 5 kí tự" +
                    $"&mess_type=error");

            var result = await _userManager.ChangePasswordAsync(appUser, model.old_password, model.new_password);
            return LocalRedirect($"/admin/identity?message=Cập nhật thành công");
        }
    }
}