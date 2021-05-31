using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class AddPostViewModel
    {
        public string[] title { get; set; }
        public string[] author { get; set; }
        public string tags { get; set; }
        public string[] post_content { get; set; }
        public string[] post_description { get; set; }
        public IFormFile thumbnail_url { get; set; }
        public int? branch_id { get; set; }
        public string date { get; set; }
        public bool? visible { get; set; }
    }

    public class UpdatePostViewModel
    {
        public string[] title { get; set; }
        public string[] author { get; set; }
        public string tags { get; set; }
        public string[] post_content { get; set; }
        public string[] post_description { get; set; }
        public IFormFile thumbnail_url { get; set; }
        public int? branch_id { get; set; }
        public string date { get; set; }
        public bool? visible { get; set; }
    }

    public class AdminPostDetailViewModel
    {
        public int Id { get; set; }
        public IDictionary<string, string> Title { get; set; }
        public IDictionary<string, string> Author { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IDictionary<string, string> PostContent { get; set; }
        public IDictionary<string, string> PostDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime? Date { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public bool? Visible { get; set; }
    }

    public class PostDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public string PostContent { get; set; }
        public string PostDescription { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime? Date { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public bool? Visible { get; set; }
    }
}
