using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Areas.Admin.Pages.Blog
{
    public class AddModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        private readonly IHostingEnvironment _env;
        public AddModel(CoffeeContext context,
            IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public override PageInfo Info => new PageInfo()
        {
            Title = "Thêm mới bài viết",
            Menu = "blog"
        };

        public IEnumerable<BranchDetailViewModel> Branches { get; set; }
        public void OnGet()
        {
            Branches = _context.Branches.OrderBy(b => b.Name).ToList().Select(b => new BranchDetailViewModel
            {
                Id = b.Id,
                Name = b.Name.GetCurrentLangString()
            }).ToList();
        }

        public IActionResult OnPost(AddPostViewModel model)
        {
            var tags = !string.IsNullOrWhiteSpace(model.tags) ?
                JsonConvert.SerializeObject(
                    model.tags.Split(',').Select(t => t.Trim())) :
                null;

            string url = null;
            if (model.thumbnail_url != null)
            {
                var uploadImgFolder = new DirectoryInfo($"{_env.WebRootPath}/uploads/imgs");
                var fullFolder = uploadImgFolder.FullName;
                if (!uploadImgFolder.Exists)
                    Directory.CreateDirectory(fullFolder);
                var fileName = new FileInfo(fullFolder + "/" + Guid.NewGuid().ToString() + ".png");
                var fullName = fileName.FullName;
                using (var fileStream = new FileStream(fullName, FileMode.Create))
                {
                    model.thumbnail_url.CopyTo(fileStream);
                }
                url = fullName.Replace(_env.WebRootPath, "");
            }

            var post = new Posts
            {
                Author = model.author.ToMultilangString().ToJson(),
                PostContent = model.post_content.ToMultilangString().ToJson(),
                PostDescription = model.post_description.ToMultilangString().ToJson(),
                Title = model.title.ToMultilangString().ToJson(),
                Tags = tags,
                ThumbnailUrl = url,
                BranchId = model.branch_id,
                Visivle =  model.visible
            };

            if (!string.IsNullOrEmpty(model.date))
            {
                DateTime date;
                if (DateTime.TryParseExact(model.date,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                    post.Date = date;
            }

            _context.Posts.Add(post);
            _context.SaveChanges();

            return LocalRedirect("/admin/blog?message=Thêm mới thành công");
        }
    }
}