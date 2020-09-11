using MemberDemo.Models;
using MemberDemo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MemberDemo.Services
{
    public class MemberSerevice
    {
        private MemberRepository _memberRepository;

        public MemberSerevice()
        {
            _memberRepository = new MemberRepository();
        }

        public async Task<Response<bool>> IsAccountExisted(string email) 
        {
            //TODO 至Redis檢查該帳號是否近期也被查詢過，降低資料庫負荷量
            var isExisted =  await _memberRepository.IsEmailExist(email);
            if (!isExisted.Success) return isExisted;
            else if (isExisted.Data)
            {
                isExisted.Message = "帳號已存在";   //更好的作法是有定義事件代碼，可以因應多語系，這邊就省略。
                //TODO Redis暫存已存在帳號 過期時間約3小時。
            }
            return isExisted;
        }

        public async Task<Response<bool>> Register(RegisterInput input)
        {
            var isExisted = await IsAccountExisted(input.Email);
            if (!isExisted.Success || isExisted.Data) return isExisted;
            var salt = GetRandomStringByGuid(5);
            input.Password = CreateMD5(input.Password + salt);
            return await _memberRepository.CreateAccount(input, salt);
        }

        public static string GetRandomStringByGuid(int length)
        {
            var str = Guid.NewGuid().ToString().Replace("-", "");
            return str.Substring(0, length);
        }

        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}