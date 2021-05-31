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
    public class SystemModel : BasePageModel<SystemModel>
    {
        public SystemModel(CoffeeContext context,
            IHtmlLocalizer<SystemModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "system",
            Title = "Hệ thống A+ Coffee",
            Description = "Hệ thống toàn bộ cửa hàng chi nhánh của A+Coffee"
        };

        public int BranchCount { get; set; }
        public List<BranchDetailViewModel> Branches { get; set; }
        public string Area { get; set; }
        public List<PostDetailViewModel> RecentPosts { get; set; }
        public List<string> AvailableAreas { get; set; }
        public void OnGet(string area = null)
        {
            Area = area;
            var hasArea = !string.IsNullOrEmpty(area);
            var activeBranches = _context.Branches
                .Where(p => p.Active == true);
            Branches = activeBranches.Where(p => !hasArea || p.Area == area)
                .Select(b => new BranchDetailViewModel
                {
                    Active = b.Active,
                    Address = b.Address.GetCurrentLangString(),
                    Description = b.Description.GetCurrentLangString(),
                    Id = b.Id,
                    Name = b.Name.GetCurrentLangString(),
                    ImageUrl = b.ImageUrl
                }).ToList();
            BranchCount = activeBranches.Count();

            AvailableAreas = _context.Branches.Where(b => b.Active == true)
                .Select(b => b.Area).Distinct().ToList();

            var post = _context.Posts.Where(p => p.Visivle == true && p.BranchId != null)
                .OrderByDescending(po => po.Date).Take(3).ToList();
            RecentPosts = post.Select(po =>
            {
                var tags = po.Tags != null ?
                JsonConvert.DeserializeObject<IEnumerable<string>>(po.Tags) : null;
                return new PostDetailViewModel
                {
                    Id = po.Id,
                    Author = po.Author.GetCurrentLangString(),
                    Tags = tags,
                    Title = po.Title.GetCurrentLangString(),
                    ThumbnailUrl = po.ThumbnailUrl?.Replace('\\', '/'),
                    PostDescription = po.PostDescription.GetCurrentLangString(),
                    Date = po.Date,
                    BranchId = po.BranchId
                };
            }).ToList();

        }
    }
}