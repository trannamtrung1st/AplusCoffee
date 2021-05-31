using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Pages
{
    public class BranchDetailModel : BasePageModel<BranchDetailModel>
    {
        
        public BranchDetailModel(CoffeeContext context,
            IHtmlLocalizer<BranchDetailModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "system",
            Title = "Thông tin chi nhánh",
            Description = $"Chi tiết chi nhánh {Branch.Name}"
        };

        public BranchDetailViewModel Branch { get; set; }
        public List<PostDetailViewModel> RecentPosts { get; set; }

        public IActionResult OnGet(int id)
        {
            var entity = _context.Branches.FirstOrDefault(b => b.Id == id);
            if (entity == null)
                return LocalRedirect("/he-thong-aplus-coffee");
            Branch = new BranchDetailViewModel
            {
                Active = entity.Active,
                Address = entity.Address.GetCurrentLangString(),
                Area = entity.Area,
                Description = entity.Description.GetCurrentLangString(),
                Id = entity.Id,
                ImageUrl = entity.ImageUrl,
                IntroContent = entity.IntroContent.GetCurrentLangString(),
                Name = entity.Name.GetCurrentLangString(),
                OpeningDate = entity.OpeningDate
            };

            RecentPosts = _context.Posts.Where(p => p.Visivle == true && p.BranchId == entity.Id)
                .OrderByDescending(p => p.Date)
                .Take(3).ToList().Select(p => new PostDetailViewModel
                {
                    Author = p.Author.GetCurrentLangString(),
                    Date = p.Date,
                    Id = p.Id,
                    PostDescription = p.PostDescription.GetCurrentLangString(),
                    ThumbnailUrl = p.ThumbnailUrl,
                    Title = p.Title.GetCurrentLangString(),
                    Tags = p.Tags != null ?
                        JsonConvert.DeserializeObject<IEnumerable<string>>(p.Tags) : null
                }).ToList();

            return Page();
        }
    }
}