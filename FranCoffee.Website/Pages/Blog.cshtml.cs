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
    public class BlogModel : BasePageModel<BlogModel>
    {

        public BlogModel(CoffeeContext context,
            IHtmlLocalizer<BlogModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "blog",
            Title = "Blog - Bài viết",
            Description = "Đến với những bài viết, tin tức chất lượng nhất của A+ Coffee"
        };

        public List<PostDetailViewModel> Posts { get; set; }
        public List<BranchDetailViewModel> Branches { get; set; }

        public int ActivePage { get; set; }
        public int TotalPage { get; set; }

        public int BranchId { get; set; }
        public void OnGet(int p = 1, int branch_id = -1)
        {
            BranchId = branch_id;
            var postPerPage = 9;
            ActivePage = p;
            var activePostQuery = _context.Posts.Where(po => po.Visivle == true);
            var count = activePostQuery.Count();
            TotalPage = count / postPerPage + 1;
            if (count % postPerPage == 0)
                TotalPage -= 1;
            Branches = _context.Branches
                .Where(b => b.Active == true)
                .OrderBy(b => b.Name)
                .Select(b => new BranchDetailViewModel
                {
                    Name = b.Name.GetCurrentLangString(),
                    Id = b.Id,
                }).ToList();

            var post = activePostQuery
                .Where(po => branch_id == -1 || (branch_id == 0 && po.BranchId != null) || po.BranchId == branch_id)
                .OrderByDescending(po => po.Date)
                .Skip((p - 1) * postPerPage).Take(postPerPage).ToList();

            Posts = post.Select(po =>
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
                    Date = po.Date
                };
            }).ToList();
        }
    }
}