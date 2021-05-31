using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Blog
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
            Menu = "blog",
            Title = "Danh sách bài viết"
        };
        public IList<PostDetailViewModel> Posts { get; set; }

        public string Message { get; set; }
        public string MessageType { get; set; }
        public void OnGet(string message = null, string mess_type = "success")
        {
            Posts = _context.Posts.OrderByDescending(p => p.Date).ToList()
                .Select(p => new PostDetailViewModel
                {
                    Author = p.Author.GetCurrentLangString(),
                    BranchId = p.BranchId,
                    Date = p.Date?.ToVNDateTime(),
                    Id = p.Id,
                    PostContent = p.PostContent.GetCurrentLangString(),
                    PostDescription = p.PostDescription.GetCurrentLangString(),
                    //Tags = p.Tags
                    ThumbnailUrl = p.ThumbnailUrl,
                    BranchName = p.Branch?.Name.GetCurrentLangString(),
                    Title = p.Title.GetCurrentLangString(),
                    Visible = p.Visivle
                }).ToList();
            Message = message;
            MessageType = mess_type;
        }
    }
}