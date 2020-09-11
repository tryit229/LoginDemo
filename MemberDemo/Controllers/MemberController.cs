using MemberDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberDemo.Controllers
{
    public class MemberController : ApiController
    {
        [HttpPost]
        public Response<string> Register(RegisterInput input)
        {

            throw new NotImplementedException();
        }

        [HttpPost]
        public Response<string> Login(LoginInput input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Response<string> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Response<string> UpdatePassword(UpdatePasswordInput input)
        {
            //確保資訊安全傳輸有幾種方式
            // 1. 以網路層擋，僅限定特定IP讀取
            // 2. 透過token協定
            throw new NotImplementedException();
        }

        [HttpGet]
        public Response<bool> IsExisted(string email)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Response<bool> Test()
        {
            return new Response<bool>() {
                Message="Hello World"
            };
        }
    }
}
