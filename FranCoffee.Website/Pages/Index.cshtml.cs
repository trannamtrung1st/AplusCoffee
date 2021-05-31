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
    public class IndexModel : BasePageModel<IndexModel>
    {
        
        public IndexModel(CoffeeContext context,
            IHtmlLocalizer<IndexModel> loc, IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "home",
            Title = L["title"].Value,
            Description = "Được đội ngũ kiến trúc sư, kỹ sư, các nhà quản lý của A+ Coffee khởi tạo và phát triển hơn 4 năm qua nhằm đem lại ly cafe nguyên chất, cafe sạch, đậm đà hương vị với không gian hiện đại, sang trọng, mát mẻ và thoải mái nhất cho khách hàng. Thiết kế quán A+ với chuỗi quán của chúng tôi được định vị như chuỗi không gian đẹp nhất, đầy đủ nhất, dễ dàng vận hành, giúp các bạn khởi nghiệp tránh rủi ro, chi phí thấp nhất."
        };


        public List<PostDetailViewModel> RecentPosts { get; set; }
        public List<BranchDetailViewModel> NewestBranches { get; set; }
        public void OnGet()
        {
            NewestBranches = _context.Branches
                .Where(b => b.Active == true).OrderByDescending(po => po.OpeningDate).Take(3)
                .Select(b => new BranchDetailViewModel
                {
                    Name = b.Name.GetCurrentLangString(),
                    Id = b.Id,
                    OpeningDate = b.OpeningDate,
                    Address = b.Address.GetCurrentLangString(),
                    Active = b.Active,
                    Area = b.Area,
                    Description = b.Description.GetCurrentLangString(),
                    ImageUrl = b.ImageUrl
                }).ToList();
            var post = _context.Posts.Where(p => p.Visivle == true).OrderByDescending(po => po.Date).Take(3).ToList();
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
