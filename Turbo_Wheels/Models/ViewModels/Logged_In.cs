using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models.ViewModels
{
    public class Logged_In
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
    }
}