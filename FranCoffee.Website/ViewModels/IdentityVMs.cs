using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FranCoffee.Website.ViewModels
{
    public class LoginViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UpdatePasswordViewModel
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
    }
}
