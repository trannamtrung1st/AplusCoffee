using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{

    public class AdminProductTypeDetailViewModel
    {
        public int Id { get; set; }
        public IDictionary<string, string> Name { get; set; }
    }

    public class ProductTypeDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
