using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberDemo.Helper
{
    public class RedisPath
    {
        public readonly static string ForgetPassword = "urn:RestPasswordToken:";
        public readonly static string EmailList = "urn:EmailList:";
        public readonly static string LoginToken = "urn:LoginToken:";
        public readonly static string LoginBlock = "urn:LoginBlock:";
    }
}