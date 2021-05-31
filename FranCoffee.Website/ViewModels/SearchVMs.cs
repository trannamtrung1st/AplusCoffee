using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class SearchResultViewModel
    {
        public string Type { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
