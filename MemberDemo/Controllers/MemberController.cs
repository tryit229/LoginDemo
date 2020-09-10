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
        public Response<string> Register()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Response<string> Login()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Response<string> ForgetPassword()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public Response<bool> IsExisted()
        {
            throw new NotImplementedException();
        }
    }
}
