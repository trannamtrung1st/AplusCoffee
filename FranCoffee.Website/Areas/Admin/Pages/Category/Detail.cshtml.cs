using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Areas.Admin.Pages.Category
{
    public class DetailModel : BasePageModel
    {
        private readonly CoffeeContext _context;
        public DetailModel(CoffeeContext context)
        {
            _context = context;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "category",
            Title = "Chi tiết danh mục"
        };

        public List<ProductTypeDetailViewModel> ProductTypes { get; set; }
        public AdminCategoryDetailViewModel Category { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public IActionResult OnGet(int id, string message = null, string mess_type = "success")
        {
            Message = message;
            MessageType = mess_type;

            var entity = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (entity == null)
                return LocalRedirect("/admin/category?message=Không tìm thấy&mess_type=error");
            Category = new AdminCategoryDetailViewModel
            {
                Id = entity.Id,
                Name = entity.Name.ToMultilangString(),
                Description = entity.Description.ToMultilangString(),
                TypeId = entity.TypeId,
                ShowOrder = entity.ShowOrder
            };
            ProductTypes = _context.ProductTypes.ToList().Select(c => new ProductTypeDetailViewModel
            {
                Id = c.Id,
                Name = c.Name.GetCurrentLangString()
            }).ToList();
            return Page();
        }

        public IActionResult OnGetDelete(int id)
        {
            try
            {
                var category = _context.Categories.FirstOrDefault(p => p.Id == id);
                if (category == null)
                    return LocalRedirect("/admin/category?message=Không tìm thấy&mess_type=error");
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return LocalRedirect("/admin/category?message=Xóa thành công");
            }
            catch (Exception e)
            {
                var mess = "Xóa thất bại. Các thực thể phụ thuộc chưa được xóa.";
                return LocalRedirect($"/admin/branch?message={mess}&mess_type=error");
            }

        }

        public IActionResult OnPost(int id, UpdateCategoryViewModel model)
        {
            var category = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (category == null)
                return LocalRedirect($"/admin/category?message=Không tìm thấy&mess_type=error");

            category.Name = model.name.ToMultilangString().ToJson();
            category.Description = model.description.ToMultilangString().ToJson();
            category.TypeId = model.type_id;
            category.ShowOrder = model.show_order;

            _context.SaveChanges();
            return LocalRedirect($"/admin/category/{id}?message=Cập nhật thành công");
        }
    }
}