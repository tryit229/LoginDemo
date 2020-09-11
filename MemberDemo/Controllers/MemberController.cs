using MemberDemo.Helper;
using MemberDemo.Models;
using MemberDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace MemberDemo.Controllers
{
    public class MemberController : ApiController
    {

        private MemberSerevice _memberSerevice;

        [HttpPost]
        [Route("api/Member/Register")]
        public async Task<Response<bool>> Register(RegisterInput input)
        {
            try
            {
                ValidateHelper.Begin()
                  .IsPasswordFomat(input.Password)
                  .IsEmail(input.Email);
                _memberSerevice = new MemberSerevice();
                var result = await _memberSerevice.Register(input);
                return result;
            }
            catch(Exception e)
            {
                return new Response<bool>()
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpPost]
        [Route("api/Member/Login")]
        public async Task<Response<string>> Login(LoginInput input)
        {
            try
            {
                ValidateHelper.Begin()
                  .IsPasswordFomat(input.Password)
                  .IsEmail(input.Email);
                _memberSerevice = new MemberSerevice();
                var result = await _memberSerevice.Login(input);
                return result;
            }
            catch (Exception e)
            {
                return new Response<string>()
                {
                    Success = false,
                    Message = e.Message,
                };
            }
        }

        [HttpPost]
        public Response<string> ForgetPassword(string email)
        {
            throw new NotImplementedException();
        }

        //[HttpPost]
        //public Response<string> UpdatePassword(UpdatePasswordInput input)
        //{
        //    //確保資訊安全傳輸有幾種方式
        //    // 1. 以網路層擋，僅限定特定IP讀取
        //    // 2. 透過token協定
        //    throw new NotImplementedException();
        //}

        [HttpGet]
        [Route("api/Member/IsAccountExisted/{email}")]
        public Response<bool> IsAccountExisted(string email)
        {
            _memberSerevice = new MemberSerevice();
            return _memberSerevice.IsAccountExisted(email).GetAwaiter().GetResult();
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
