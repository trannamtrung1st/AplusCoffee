using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class FranchiseRegisterViewModel
    {
        public string full_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
    }

    public class ContactViewModel
    {
        public string full_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }

}
