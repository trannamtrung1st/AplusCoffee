using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FranCoffee.Website.Areas.Admin.Pages.Product
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
            Title = "Thêm mới sản phẩm",
            Menu = "product"
        };

        public List<CategoryDetailViewModel> Categories { get; set; }
        public void OnGet()
        {
            Categories = _context.Categories.OrderBy(c => c.ShowOrder).ToList().Select(c => new CategoryDetailViewModel
            {
                Id = c.Id,
                Name = c.Name.GetCurrentLangString(),
                TypeName = c.Type.Name.GetCurrentLangString()
            }).ToList();
        }

        private string SaveImage(IFormFile file)
        {
            string url = null;
            var uploadImgFolder = new DirectoryInfo($"{_env.WebRootPath}/uploads/imgs/products");
            var fullFolder = uploadImgFolder.FullName;
            if (!uploadImgFolder.Exists)
                Directory.CreateDirectory(fullFolder);
            var fileName = new FileInfo(fullFolder + "/" + Guid.NewGuid().ToString() + ".png");
            var fullName = fileName.FullName;
            using (var fileStream = new FileStream(fullName, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            url = fullName.Replace(_env.WebRootPath, "");

            return url;
        }

        public IActionResult OnPost(AddProductViewModel model)
        {
            var list = new List<ProductImagesViewModel>();
            if (model.avatar != null)
                list.Add(new ProductImagesViewModel
                {
                    IsAvatar = true,
                    RelPath = SaveImage(model.avatar)
                });

            if (model.detail != null)
                foreach (var f in model.detail)
                    list.Add(new ProductImagesViewModel
                    {
                        RelPath = SaveImage(f),
                        IsAvatar = false
                    });
            var images = JsonConvert.SerializeObject(list);

            var pro = new Products
            {
                Active = model.active,
                CategoryId = model.category_id,
                Description = model.description.ToMultilangString().ToJson(),
                Name = model.name.ToMultilangString().ToJson(),
                Images = images,
                ShowOrder = model.show_order
            };

            _context.Products.Add(pro);
            _context.SaveChanges();
            return LocalRedirect("/admin/product?message=Thêm mới thành công");
        }
    }
}