using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Branch
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
            Menu = "branch",
            Title = "Chi tiết chi nhánh"
        };

        public AdminBranchDetailViewModel Branch { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public IActionResult OnGet(int id, string message = null, string mess_type = "success")
        {
            Message = message;
            MessageType = mess_type;

            var entity = _context.Branches.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return LocalRedirect("/admin/branch?message=Không tìm thấy&mess_type=error");
            Branch = new AdminBranchDetailViewModel
            {
                Id = entity.Id,
                Name = entity.Name.ToMultilangString(),
                Description = entity.Description.ToMultilangString(),
                Address = entity.Address.ToMultilangString(),
                ImageUrl = entity.ImageUrl,
                Active = entity.Active,
                Area = entity.Area,
                OpeningDate = entity.OpeningDate?.ToVNDateTime(),
                IntroContent = entity.IntroContent.ToMultilangString()
            };
            return Page();
        }

        private string SaveImage(IFormFile file)
        {
            var uploadImgFolder = new DirectoryInfo($"{_env.WebRootPath}/uploads/imgs/branches");
            var fullFolder = uploadImgFolder.FullName;
            if (!uploadImgFolder.Exists)
                Directory.CreateDirectory(fullFolder);
            var fileName = new FileInfo(fullFolder + "/" + Guid.NewGuid().ToString() + ".png");
            var fullName = fileName.FullName;
            using (var fileStream = new FileStream(fullName, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            var url = fullName.Replace(_env.WebRootPath, "");
            return url;
        }


        public IActionResult OnGetDelete(int id)
        {
            try
            {


                var branch = _context.Branches.FirstOrDefault(p => p.Id == id);
                if (branch == null)
                    return LocalRedirect("/admin/branch?message=Không tìm thấy&mess_type=error");
                _context.Branches.Remove(branch);
                _context.SaveChanges();

                FileInfo file = null;
                if (branch.ImageUrl != null && (file = new FileInfo(_env.WebRootPath + branch.ImageUrl)).Exists)
                    file.Delete();

                return LocalRedirect("/admin/branch?message=Xóa thành công");
            }
            catch (Exception e)
            {
                var mess = "Xóa thất bại. Các thực thể phụ thuộc chưa được xóa.";
                return LocalRedirect($"/admin/branch?message={mess}&mess_type=error");
            }
        }

        public IActionResult OnPost(int id, UpdateBranchViewModel model)
        {
            var branch = _context.Branches.FirstOrDefault(p => p.Id == id);
            if (branch == null)
                return LocalRedirect($"/admin/branch?message=Không tìm thấy&mess_type=error");

            branch.Name = model.name.ToMultilangString().ToJson();
            branch.Description = model.description.ToMultilangString().ToJson();
            branch.Active = model.active;
            branch.Address = model.address.ToMultilangString().ToJson();
            branch.Area = model.area;
            branch.IntroContent = model.intro_content.ToMultilangString().ToJson();

            if (!string.IsNullOrEmpty(model.opening_date))
            {
                DateTime date;
                if (DateTime.TryParseExact(model.opening_date,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                    branch.OpeningDate = date;
            }

            if (model.image_url != null)
            {
                var url = SaveImage(model.image_url);
                FileInfo oldFile;
                if (branch.ImageUrl != null &&
                    (oldFile = new FileInfo(_env.WebRootPath + branch.ImageUrl)).Exists)
                {
                    oldFile.Delete();
                }
                branch.ImageUrl = url;
            }

            _context.SaveChanges();
            return LocalRedirect($"/admin/branch/{id}?message=Cập nhật thành công");
        }
    }
}