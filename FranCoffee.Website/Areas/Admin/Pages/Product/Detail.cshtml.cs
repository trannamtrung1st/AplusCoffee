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
            Menu = "product",
            Title = "Chi tiết sản phẩm"
        };

        public AdminProductDetailViewModel Product { get; set; }
        public List<CategoryDetailViewModel> Categories { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public IActionResult OnGet(int id, string message = null, string mess_type = "success")
        {
            Message = message;
            MessageType = mess_type;

            Categories = _context.Categories.OrderBy(c => c.ShowOrder).ToList().Select(c => new CategoryDetailViewModel
            {
                Id = c.Id,
                Name = c.Name.GetCurrentLangString(),
                TypeName = c.Type.Name.GetCurrentLangString()
            }).ToList();

            var entity = _context.Products.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return LocalRedirect("/admin/product?message=Không tìm thấy&mess_type=error");
            var images = entity.Images != null ?
                JsonConvert.DeserializeObject<IEnumerable<ProductImagesViewModel>>(entity.Images) : null;
            Product = new AdminProductDetailViewModel
            {
                Id = entity.Id,
                Images = images,
                Active = entity.Active,
                CategoryId = entity.CategoryId,
                Description = entity.Description.ToMultilangString(),
                Name = entity.Name.ToMultilangString(),
                ShowOrder = entity.ShowOrder,
            };
            return Page();
        }

        public IActionResult OnGetDelete(int id)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);
                if (product == null)
                    return LocalRedirect("/admin/product?message=Không tìm thấy&mess_type=error");
                _context.Products.Remove(product);
                _context.SaveChanges();
                if (product.Images != null)
                {
                    var list = JsonConvert.DeserializeObject<IEnumerable<ProductImagesViewModel>>(product.Images);
                    foreach (var i in list)
                    {
                        var url = _env.WebRootPath + i.RelPath;
                        var f = new FileInfo(url);
                        if (f.Exists)
                            f.Delete();
                    }

                }
                return LocalRedirect("/admin/product?message=Xóa thành công");
            }
            catch (Exception e)
            {
                var mess = "Xóa thất bại. Các thực thể phụ thuộc chưa được xóa.";
                return LocalRedirect($"/admin/branch?message={mess}&mess_type=error");
            }
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

        public IActionResult OnPost(int id, UpdateProductViewModel model)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return LocalRedirect($"/admin/product?message=Không tìm thấy&mess_type=error");

            List<ProductImagesViewModel> oldImgs = new List<ProductImagesViewModel>();
            if (product.Images != null)
                oldImgs = JsonConvert.DeserializeObject<List<ProductImagesViewModel>>(product.Images);

            var list = new List<ProductImagesViewModel>();
            if (model.avatar != null)
            {
                list.Add(new ProductImagesViewModel
                {
                    IsAvatar = true,
                    RelPath = SaveImage(model.avatar)
                });
                var delete = oldImgs.FirstOrDefault(i => i.IsAvatar);
                if (delete != null)
                {
                    oldImgs.Remove(delete);
                    var f = new FileInfo(_env.WebRootPath + delete.RelPath);
                    if (f.Exists) f.Delete();
                }
            }

            if (model.detail != null)
            {
                foreach (var f in model.detail)
                    list.Add(new ProductImagesViewModel
                    {
                        RelPath = SaveImage(f),
                        IsAvatar = false
                    });
                var delete = oldImgs.Where(i => !i.IsAvatar).ToList();
                foreach (var del in delete)
                {
                    var f = new FileInfo(_env.WebRootPath + del.RelPath);
                    if (f.Exists) f.Delete();
                    oldImgs.Remove(del);
                }
            }
            list.AddRange(oldImgs);
            var images = JsonConvert.SerializeObject(list);

            product.Active = model.active;
            product.CategoryId = model.category_id;
            product.Description = model.description.ToMultilangString().ToJson();
            product.Images = images;
            product.Name = model.name.ToMultilangString().ToJson();
            product.ShowOrder = model.show_order;

            _context.SaveChanges();
            return LocalRedirect($"/admin/product/{id}?message=Cập nhật thành công");
        }
    }
}