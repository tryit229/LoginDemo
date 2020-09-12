using MemberDemo.Helper;
using MemberDemo.Models;
using MemberDemo.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var isExisted =  await GetAccountID(email);
            if (string.IsNullOrEmpty(isExisted.Data))
            {
                //帳號不存在
                return new Response<bool>()
                {
                    Success = isExisted.Success,
                    Data = false,
                    Message = isExisted.Message
                };
            }
            else
            {
                return new Response<bool>() 
                { 
                    Success = isExisted.Success, 
                    Data = true, 
                    Message = isExisted.Success ? "帳號已存在" : isExisted.Message   //更好的作法是有定義事件代碼，可以因應多語系，這邊就省略。
                };   
            }
        }

        public async Task<Response<string>> GetAccountID(string email)
        {
            //TODO 至Redis檢查該帳號是否近期也被查詢過，降低資料庫負荷量
            return await _memberRepository.GetAccountIDByEmail(email);
        }

        public async Task<Response<string>> Login(LoginInput input)
        {
            var authData = await _memberRepository.LoginData(input.Email);
            if(authData.Data.Password == PasswordHash(input.Password, authData.Data.Salt))
            {
                var token = Guid.NewGuid().ToString();
                //TODO 存Token到Redis LoginToken 3HR
                return new Response<string>()
                {
                    Success = true,
                    Data = token,
                    Message = "登入成功"
                };
            }
            //TODO 同一Email 短時間嘗試超過次數　鎖定機制
            return new Response<string>()
            {
                Success = true,
                Message = "登入失敗"
            };
        }

        public async Task<Response<bool>> Register(RegisterInput input)
        {
            var isExisted = await IsAccountExisted(input.Email);
            if (!isExisted.Success || isExisted.Data) return isExisted; //失敗|帳號已存在
            var salt = GetRandomStringByGuid(5);
            input.Password = PasswordHash(input.Password, salt);
            return await _memberRepository.CreateAccount(input, salt);
        }

        public async Task<Response<bool>> ForgetPassword(string email)
        {
            var isExisted = await IsAccountExisted(email);
            if (!isExisted.Success || !isExisted.Data) return isExisted;    //失敗|帳號不存在
            var token = (Guid.NewGuid().ToString()).Replace("-","");
            var setTokenTask = SetResetPasswordTokenToRedis(email, token);
            var sendMailTask = SendForgetPasswordMail(email, token);
            await Task.WhenAll(setTokenTask, sendMailTask);
            if(setTokenTask.Result.Success && sendMailTask.Result)
            {
                return new Response<bool>()
                {
                    Success = true,
                    Data = true,
                    Message = "成功"
                };
            }
            return new Response<bool>()
            {
                Success = false,
                Data = false,
                Message = $"設Token {setTokenTask.Result.Success} ; 寄郵件 {sendMailTask.Result} "
            };
        }

        public async Task<Response<bool>> SetResetPasswordTokenToRedis(string email, string token)
        {
            var accountIDTask = await GetAccountID(email); //選用ID當Key的原因: 1. ID是資料庫的PK　2. 不希望太容易被識別出來
            if(accountIDTask.Success && !string.IsNullOrEmpty(accountIDTask.Data))
                return await RedisHelper.SetKeyValueToRedis(ConfigurationManager.AppSettings["RedisSetting"], $"{RedisPath.ForgetPassword}{token}", accountIDTask.Data, 5);
            return new Response<bool>()
            {
                Success = false,
                Message = "查無此帳號"
            };
        }

        public async Task<bool> SendForgetPasswordMail(string email, string token)
        {
            //假裝有送Email XD
            return true;
        }

        public async Task<Response<bool>> UpdatePassword(UpdatePasswordInput input)
        {
            //TODO 確認 ResetPasswordToken
            //TODO 更新密碼、寫LOG
            //var accountIDTask = await GetAccountID(input.Email);    //TODO OR Token Get AccountID
            var accountIDTask = await RedisHelper.GetKeyValueFromRedis(ConfigurationManager.AppSettings["RedisSetting"], $"{RedisPath.ForgetPassword}{input.Token}");
            if (!accountIDTask.Success || string.IsNullOrEmpty(accountIDTask.Data))
            {
                return new Response<bool>()
                {
                    Success = false,
                    Message = "Token Expired or Not Existed"
                };
            }
            var salt = GetRandomStringByGuid(5);
            input.Password = PasswordHash(input.Password, salt);
            var updatePasswordTask = _memberRepository.UpdatePassword(accountIDTask.Data, input.Password , salt);
            var deleteForgetPwdTokenTask = RedisHelper.DeleteFromRedis(ConfigurationManager.AppSettings["RedisSetting"], $"{RedisPath.ForgetPassword}{input.Token}");
            await Task.WhenAll(updatePasswordTask, deleteForgetPwdTokenTask);
            if (updatePasswordTask.Result.Success && deleteForgetPwdTokenTask.Result.Success)
            {
                return new Response<bool>()
                {
                    Success = true,
                    Data = true,
                    Message = "成功"
                };
            }
            return new Response<bool>()
            {
                Success = false,
                Data = false,
                Message = $"設Token {updatePasswordTask.Result.Success} ; 寄郵件 {deleteForgetPwdTokenTask.Result.Success} "
            };
        }

        public static string GetRandomStringByGuid(int length)
        {
            var str = Guid.NewGuid().ToString().Replace("-", "");
            return str.Substring(0, length);
        }

        public static string PasswordHash(string pwd, string salt)
        {
            return CreateMD5(pwd + salt);
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