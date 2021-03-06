﻿using Dapper;
using MemberDemo.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MemberDemo.Repository
{
    public class MemberRepository
    {
        private readonly string _connectionString;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public MemberRepository() 
        {
            _connectionString = ConfigurationManager.AppSettings["DBSetting"];

        }

        private SqlConnection GetOpenConnection()
        {
            return new SqlConnection(this._connectionString);
        }

        public async Task<Response<string>> GetAccountIDByEmail(string email)
        {
            try
            {
                using (var cn = GetOpenConnection())
                {
                    var result = await cn.QueryAsync<string>
                        (@" SELECT [ID]
                          FROM [Member] (NOLOCK)
                          WHERE [Email]=@Email ", new { Email = email }
                         ).ConfigureAwait(continueOnCapturedContext: false);

                    return new Response<string>(){
                        Success = true,
                        Data = result.FirstOrDefault()
                    };
                }
            }
            catch (Exception e)
            {
                var errStr = $"IsEmailExist:{e.Message}";
                _logger.Error(errStr);
                return new Response<string>()
                {
                    Success = false,
                    Message = errStr
                };
            }
        }

        public async Task<Response<bool>> CreateAccount(RegisterInput input, string salt)
        {
            try
            {
                using (var cn = GetOpenConnection())
                {
                    var result = await cn.ExecuteAsync(
                        @"INSERT INTO [Member]
                          (   [Email]
                              ,[Password]
                              ,[Salt]
                              ,[Name]
                              ,[Birthday]
                              ,[AreaCode]
                              ,[Mobile]
                              ,[Country]
                              ,[City]
                              ,[District]
                              ,[Address])
                          VALUES(@Email,@Password,@Salt,@Name,@Birthday,@AreaCode,@Mobile,@Country,@City,@District,@Address)",
                        new { Email = input.Email,
                            Password = input.Password,
                            Salt = salt,
                            Name = input.Name,
                            Birthday = input.Birthday,
                            AreaCode = input.AreaCode,
                            Mobile = input.Mobile,
                            Country = input.Country,
                            City = input.City,
                            District = input.District,
                            Address = input.Address
                        }
                        );
                    return new Response<bool>()
                    {
                        Success = result > 0,
                        Data = result > 0
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Error($"CreateAccount:{e.Message}");
                return new Response<bool>()
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response<bool>> UpdatePassword(string id , string password, string salt)
        {
            try
            {
                using (var cn = GetOpenConnection())
                {
                    var result = await cn.ExecuteAsync(
                        @"UPDATE [Member] SET [Password]=@Password, [Salt] = @Salt WHERE [ID] = @ID",
                        new { Password = password, ID = id, Salt = salt }
                         ).ConfigureAwait(continueOnCapturedContext: false);

                    return new Response<bool>()
                    {
                        Success = result > 0,
                        Data = result>0
                    };
                }
            }
            catch (Exception e)
            {
                _logger.Error($"UpdatePassword:{e.Message}");
                return new Response<bool>()
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        public async Task<Response<MemberDAO>> LoginData(string email)
        {
            try
            {
                using (var cn = GetOpenConnection())
                {
                    var result = await cn.QueryAsync<MemberDAO>
                        (@" SELECT [ID]
                              ,[Email]
                              ,[Password]
                              ,[Salt]
                              ,[Name]
                              ,[Birthday]
                              ,[AreaCode]
                              ,[Mobile]
                              ,[Country]
                              ,[City]
                              ,[District]
                              ,[Address]
                              ,[Status]
                              ,[CreateTime]
                        FROM [Member]", new { Email = email }
                         ).ConfigureAwait(continueOnCapturedContext: false);

                    return new Response<MemberDAO>()
                    {
                        Success = true,
                        Data = result.FirstOrDefault()
                    };
                }
            }
            catch (Exception e)
            {
                var errStr = $"LoginData:{e.Message}";
                _logger.Error(errStr);
                return new Response<MemberDAO>()
                {
                    Success = false,
                    Message = errStr
                };
            }
        }

    }
}