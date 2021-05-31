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
    public class DetailModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        private readonly IHostingEnvironment _env;
        public DetailModel(CoffeeContext context,
            IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "blog",
            Title = "Chi tiết bài viết"
        };

        public AdminPostDetailViewModel Post { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public IEnumerable<BranchDetailViewModel> Branches { get; set; }

        public IActionResult OnGet(int id, string message = null, string mess_type = "success")
        {
            Message = message;
            MessageType = mess_type;

            var entity = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return LocalRedirect("/admin/blog?message=Không tìm thấy&mess_type=error");
            Branches = _context.Branches.OrderBy(b => b.Name).ToList().Select(b => new BranchDetailViewModel
            {
                Id = b.Id,
                Name = b.Name.GetCurrentLangString()
            }).ToList();

            var tags = entity.Tags != null ?
                JsonConvert.DeserializeObject<IEnumerable<string>>(entity.Tags) : null;
            Post = new AdminPostDetailViewModel
            {
                Id = entity.Id,
                Author = entity.Author.ToMultilangString(),
                PostDescription = entity.PostDescription.ToMultilangString(),
                PostContent = entity.PostContent.ToMultilangString(),
                Tags = tags,
                Title = entity.Title.ToMultilangString(),
                ThumbnailUrl = entity.ThumbnailUrl,
                BranchId = entity.BranchId,
                Date = entity.Date?.ToVNDateTime(),
                Visible = entity.Visivle
            };
            return Page();
        }

        public IActionResult OnGetDelete(int id)
        {
            try
            {
                var post = _context.Posts.FirstOrDefault(p => p.Id == id);
                if (post == null)
                    return LocalRedirect("/admin/blog?message=Không tìm thấy&mess_type=error");
                _context.Posts.Remove(post);
                _context.SaveChanges();
                FileInfo file = null;
                if (post.ThumbnailUrl != null && (file = new FileInfo(_env.WebRootPath + post.ThumbnailUrl)).Exists)
                    file.Delete();
                return LocalRedirect("/admin/blog?message=Xóa thành công");
            }
            catch (Exception e)
            {
                var mess = "Xóa thất bại. Các thực thể phụ thuộc chưa được xóa.";
                return LocalRedirect($"/admin/blog?message={mess}&mess_type=error");
            }

        }

        public IActionResult OnPost(int id, UpdatePostViewModel model)
        {
            var tags = !string.IsNullOrWhiteSpace(model.tags) ?
                JsonConvert.SerializeObject(
                    model.tags.Split(',').Select(t => t.Trim())) :
                null;
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return LocalRedirect($"/admin/blog?message=Không tìm thấy&mess_type=error");

            var oldThumbnail = post.ThumbnailUrl;
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

                FileInfo oldFile;
                if (oldThumbnail != null &&
                    (oldFile = new FileInfo(_env.WebRootPath + oldThumbnail)).Exists)
                {
                    oldFile.Delete();
                }
                post.ThumbnailUrl = url;
            }

            post.PostContent = model.post_content.ToMultilangString().ToJson();
            post.Tags = tags;
            post.Title = model.title.ToMultilangString().ToJson();
            post.Author = model.author.ToMultilangString().ToJson();
            post.PostDescription = model.post_description.ToMultilangString().ToJson();
            post.BranchId = model.branch_id;
            post.Visivle = model.visible;

            if (!string.IsNullOrEmpty(model.date))
            {
                DateTime date;
                if (DateTime.TryParseExact(model.date,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                    post.Date = date;
            }

            _context.SaveChanges();
            return LocalRedirect($"/admin/blog/{id}?message=Cập nhật thành công");
        }
    }
}