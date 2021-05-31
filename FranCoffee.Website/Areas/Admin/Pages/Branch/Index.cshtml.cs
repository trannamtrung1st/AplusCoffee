using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Branch
{
    public class IndexModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        public IndexModel(CoffeeContext context)
        {
            _context = context;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "branch",
            Title = "Danh sách chi nhánh"
        };
        public IList<BranchDetailViewModel> Branches { get; set; }

        public string Message { get; set; }
        public string MessageType { get; set; }
        public void OnGet(string message = null, string mess_type = "success")
        {
            Branches = _context.Branches.OrderByDescending(p => p.OpeningDate).ToList()
                .Select(b=> new BranchDetailViewModel
                {
                    Id = b.Id,
                    Name = b.Name.GetCurrentLangString(),
                    Description = b.Description.GetCurrentLangString(),
                    Address = b.Address.GetCurrentLangString(),
                    ImageUrl = b.ImageUrl,
                    Active = b.Active,
                    Area = b.Area,
                    OpeningDate = b.OpeningDate?.ToVNDateTime(),
                }).ToList();
            Message = message;
            MessageType = mess_type;
        }
    }
}