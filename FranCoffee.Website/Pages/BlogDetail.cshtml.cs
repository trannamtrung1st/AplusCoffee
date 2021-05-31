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
    public class BlogDetailModel : BasePageModel<BlogDetailModel>
    {

        public BlogDetailModel(CoffeeContext context,
            IHtmlLocalizer<BlogDetailModel> loc, IHtmlLocalizer<App> sharedL) : base(context,loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "blog",
            Title = "Chi tiết bài viết",
            Description = Entity.PostDescription?.ToMultilangString()["vi"]
        };

        public PostDetailViewModel Post { get; set; }
        public List<PostDetailViewModel> RecentPosts { get; set; }
        private Posts Entity { get; set; }

        public IActionResult OnGet(int id)
        {
            Entity = _context.Posts.FirstOrDefault(p => p.Visivle == true && p.Id == id);
            if (Entity == null)
                return LocalRedirect("/bai-viet");
            var tags = Entity.Tags != null ?
                JsonConvert.DeserializeObject<IEnumerable<string>>(Entity.Tags) : null;
            Post = new PostDetailViewModel
            {
                Author = Entity.Author.GetCurrentLangString(),
                Date = Entity.Date,
                Id = Entity.Id,
                PostContent = Entity.PostContent.GetCurrentLangString(),
                PostDescription = Entity.PostDescription.GetCurrentLangString(),
                Tags = tags,
                ThumbnailUrl = Entity.ThumbnailUrl,
                Title = Entity.Title.GetCurrentLangString()
            };

            RecentPosts = _context.Posts.Where(p => p.Visivle == true && p.Id != Entity.Id)
                .OrderByDescending(p => p.Date)
                .Take(4).Select(p => new PostDetailViewModel
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