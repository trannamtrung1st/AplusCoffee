using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Pages
{
    public class MailModel : BasePageModel<MailModel>
    {
        public MailModel(CoffeeContext context, 
            IHtmlLocalizer<MailModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public void OnGet(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }

        public override PageInfo Info => new PageInfo
        {
            Menu = null,
            Title = L["title"].Value,
            Description = "Kết quả gửi mail"
        };

        public async Task<IActionResult> OnPostRegister(FranchiseRegisterViewModel model)
        {
            try
            {
                var body =
                $"<b>Họ tên</b>: {model.full_name}<br/>" +
                $"<b>Email</b>: {model.email}<br/>" +
                $"<b>Số điện thoại</b>: {model.phone}<br/>" +
                $"<b>Tin nhắn</b>: {model.message}<br/>";
                await App.Instance.GmailManager.SendEmail(
                    App.Instance.ToMailAddress, "Thư đăng kí nhượng quyền mới",
                    body);
                return LocalRedirect($"/mail?success=true");
            }
            catch (Exception e)
            {
                return LocalRedirect($"/mail?success=false");
            }
        }
        public async Task<IActionResult> OnPostContact(ContactViewModel model)
        {
            try
            {
                var body =
                $"<b>Họ tên</b>: {model.full_name}<br/>" +
                $"<b>Email</b>: {model.email}<br/>" +
                $"<b>Số điện thoại</b>: {model.phone}<br/>" +
                $"<b>Tin nhắn</b>: {model.message}<br/>";
                await App.Instance.GmailManager.SendEmail(
                    App.Instance.ToMailAddress, $"Thư liên hệ: {model.subject}",
                    body);
                return LocalRedirect($"/mail?success=true");
            }
            catch (Exception e)
            {
                return LocalRedirect($"/mail?success=false");
            }
        }
    }
}