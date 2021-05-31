using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FranCoffee.Website.Models;
using FranCoffee.Website.Pages.Shared;
using FranCoffee.Website.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FranCoffee.Website.Pages
{
    public class SearchResultModel : BasePageModel<SearchResultModel>
    {
        private readonly IHtmlLocalizer<Layout> _layoutL;
        public SearchResultModel(
            CoffeeContext context,
            IHtmlLocalizer<SearchResultModel> loc,
            IHtmlLocalizer<Layout> layoutL,
            IHtmlLocalizer<App> sharedL) : base(context, loc, sharedL)
        {
            _layoutL = layoutL;
        }

        public override PageInfo Info => new PageInfo()
        {
            Menu = "",
            Title = "Kết quả tìm kiếm",
            Description = "Kết quả tìm kiếm cho bạn"
        };
        public string Query { get; set; }

        public List<SearchResultViewModel> Results { get; set; }

        private IDictionary<string, SearchResultViewModel> Pages;

        public void OnGet(string q)
        {
            Query = q;
            Results = new List<SearchResultViewModel>();

            if (string.IsNullOrWhiteSpace(q))
                return;
            q = q.Trim();

            var branches = _context.Branches.Where(b =>
                b.Name.Contains(q)
                || b.Address.Contains(q)
                || b.Area.Contains(q))
                .Select(b => new SearchResultViewModel
                {
                    Title = b.Name.GetCurrentLangString(),
                    Path = $"/thong-tin-cua-hang/{b.Id}",
                    Description = b.Description.GetCurrentLangString(),
                    Type = L["type_branch"].Value
                }).ToList();
            Results.AddRange(branches);

            //categories, products...
            var pTypes = _context.ProductTypes.Where(
                t => t.Name.Contains(q)).Select(p => new SearchResultViewModel
                {
                    Title = p.Name.GetCurrentLangString(),
                    Path = $"/san-pham?type={p.Id}",
                    Type = L["type_page"].Value
                });
            Results.AddRange(pTypes);

            var products = _context.Products.OrderBy(c => c.ShowOrder)
                .OrderBy(c => c.Name).Where(
                t => t.Name.Contains(q)).Select(p => new SearchResultViewModel
                {
                    Title = p.Name.GetCurrentLangString(),
                    Path = $"/san-pham/{p.Id}",
                    Description = p.Description.GetCurrentLangString(),
                    Type = L["type_product"].Value
                });
            Results.AddRange(products);

            var cateProducts = _context.Categories.OrderBy(c => c.ShowOrder).Where(
                c => c.Name.Contains(q)).Select(c => c.Products);
            foreach (var p in cateProducts)
            {
                var res = p.Select(pr => new SearchResultViewModel
                {
                    Description = pr.Description.GetCurrentLangString(),
                    Path = $"/san-pham/{pr.Id}",
                    Type = L["type_product"].Value,
                    Title = pr.Name.GetCurrentLangString(),
                });
                Results.AddRange(res);
            }

            var posts = _context.Posts.Where(p => p.Visivle == true
            && p.Author.Contains(q)
            || p.PostDescription.Contains(q)
            || p.Tags.Contains(q)
            || p.Title.Contains(q)).Select(p => new SearchResultViewModel
            {
                Title = p.Title.GetCurrentLangString(),
                Path = $"/chi-tiet-bai-viet/{p.Id}",
                Description = p.PostDescription.GetCurrentLangString(),
                Type = L["type_post"].Value
            }).ToList();
            Results.AddRange(posts);

            //pages
            Pages = new Dictionary<string, SearchResultViewModel>
            {
                { "/ve-chung-toi",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/ve-chung-toi",
                        Title = _layoutL["menu_about_us"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/bai-viet",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/bai-viet",
                        Title = _layoutL["menu_blog"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/lien-he",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/lien-he",
                        Title = _layoutL["menu_contact"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/san-pham",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/san-pham",
                        Title = _layoutL["menu_product"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/gia-tri-cot-loi",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/gia-tri-cot-loi",
                        Title = _layoutL["menu_au_core"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/nhuong-quyen",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/nhuong-quyen",
                        Title = _layoutL["menu_franchise"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/",
                        Title = _layoutL["menu_home"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/co-hoi-nghe-nghiep",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/co-hoi-nghe-nghiep",
                        Title = _layoutL["menu_au_job"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/he-thong-aplus-coffee",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/he-thong-aplus-coffee",
                        Title = _layoutL["menu_system"].Value,
                        Type= L["type_page"].Value
                    }
                },
                { "/tam-nhin-su-menh",
                    new SearchResultViewModel{
                        Description = null,
                        Path = "/tam-nhin-su-menh",
                        Title = _layoutL["menu_au_vision"].Value,
                        Type= L["type_page"].Value
                    }
                },
            };
            //query pages
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            var compareOptions = CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase;

            var filtered = Pages.Values.Where(
                  p => (p.Description != null && compareInfo.IndexOf(p.Description, q, compareOptions) > -1)
                  || compareInfo.IndexOf(p.Title, q, compareOptions) > -1
                  || compareInfo.IndexOf(p.Type, q, compareOptions) > -1).ToList();

            Results.AddRange(filtered);
            Results = Results.Distinct(new ResultComparer()).ToList();
        }

        private class ResultComparer : IEqualityComparer<SearchResultViewModel>
        {
            public bool Equals(SearchResultViewModel x, SearchResultViewModel y)
            {
                return x.Path.Equals(y.Path);
            }

            public int GetHashCode(SearchResultViewModel obj)
            {
                return obj.Path.GetHashCode();
            }
        }
    }
}