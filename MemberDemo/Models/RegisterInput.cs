using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class RegisterInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }
        public string AreaCode { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
    }
}