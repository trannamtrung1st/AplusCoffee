using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class AddBranchViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public string[] address { get; set; }
        public IFormFile image_url { get; set; }
        public bool? active { get; set; }
        public string area { get; set; }
        public string[] intro_content { get; set; }
        public string opening_date { get; set; }
    }

    public class UpdateBranchViewModel
    {
        public string[] name { get; set; }
        public string[] description { get; set; }
        public string[] address { get; set; }
        public IFormFile image_url { get; set; }
        public bool? active { get; set; }
        public string area { get; set; }
        public string[] intro_content { get; set; }
        public string opening_date { get; set; }
    }

    public class AdminBranchDetailViewModel
    {
        public int Id { get; set; }
        public IDictionary<string, string> Name { get; set; }
        public IDictionary<string, string> Description { get; set; }
        public IDictionary<string, string> Address { get; set; }
        public string ImageUrl { get; set; }
        public bool? Active { get; set; }
        public string Area { get; set; }
        public IDictionary<string, string> IntroContent { get; set; }
        public DateTime? OpeningDate { get; set; }
    }

    public class BranchDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public bool? Active { get; set; }
        public string Area { get; set; }
        public string IntroContent { get; set; }
        public DateTime? OpeningDate { get; set; }
    }
}
