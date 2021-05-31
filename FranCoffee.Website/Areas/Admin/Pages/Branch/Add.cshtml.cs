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
            Title = "Thêm mới chi nhánh",
            Menu = "branch"
        };

        public void OnGet()
        {
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

        public IActionResult OnPost(AddBranchViewModel model)
        {

            string url = null;
            if (model.image_url != null)
            {
                url = SaveImage(model.image_url);
            }

            var branch = new Branches
            {
                Description = model.description.ToMultilangString().ToJson(),
                Name = model.name.ToMultilangString().ToJson(),
                Active = model.active,
                Address = model.address.ToMultilangString().ToJson(),
                ImageUrl = url,
                Area = model.area,
                IntroContent = model.intro_content.ToMultilangString().ToJson(),
            };

            if (!string.IsNullOrEmpty(model.opening_date))
            {
                DateTime date;
                if (DateTime.TryParseExact(model.opening_date,
                    "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out date))
                    branch.OpeningDate = date;
            }

            _context.Branches.Add(branch);
            _context.SaveChanges();

            return LocalRedirect("/admin/branch?message=Thêm mới thành công");
        }
    }
}