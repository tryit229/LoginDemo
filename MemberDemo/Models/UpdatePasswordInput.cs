using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberDemo.Models
{
    public class UpdatePasswordInput
    {
        //public UpdatePasswordAction Action { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
    }

    //public enum UpdatePasswordAction
    //{
    //    Unknown,
    //    MemberCenter,   //用戶從前台修改密碼
    //    ForgetPassword  //忘記密碼
    //}
}