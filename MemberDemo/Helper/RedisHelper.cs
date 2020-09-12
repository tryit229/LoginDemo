using MemberDemo.Models;
using NLog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MemberDemo.Helper
{
    public class RedisHelper
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static IDatabase GetRedisDatabase(string redisEndPoints)
        {
            try
            {
                RedisConnection.Init(redisEndPoints);

                var redis = RedisConnection.Instance.ConnectionMultiplexer;

                return redis.GetDatabase();
            }
            catch (Exception ex)
            {
                _logger.Error($"GetRedisDatabase:  {ex.Message}");
                return null;
            }
        }

        public static async Task<Response<bool>> SetKeyValueToRedis(string redisEndPoints, string key, string value, int expiry)
        {
            var msg = string.Empty;
            try
            {
                var database = GetRedisDatabase(redisEndPoints);
                if (database != null)
                {
                    var result = await database.StringSetAsync(key, value, new TimeSpan(0, expiry, 0));
                    return new Response<bool>()
                    {
                        Success = true,
                        Data = result
                    };
                }
            }
            catch (Exception ex)
            {
                msg = $"SetKeyValueToRedis:key={key} value={value} | {ex.Message}";
                _logger.Error(msg);
            }
            return new Response<bool>()
            {
                Success = false,
                Message = String.IsNullOrEmpty(msg) ? $"Can't get Redis DB {redisEndPoints}" : msg
            };
        }

        public static async Task<Response<string>> GetKeyValueFromRedis(string redisEndPoints, string key)
        {
            var msg = string.Empty;
            try
            {
                var database = GetRedisDatabase(redisEndPoints);
                if (database != null)
                {
                    var result = await database.StringGetAsync(key, CommandFlags.PreferSlave);
                    if (result.IsNullOrEmpty)
                        return new Response<string>()
                        {
                            Success = false,
                            Message = $"Not Found"
                        };
                    else
                        return new Response<string>()
                        {
                            Success = true,
                            Data = result.ToString()
                        };
                }
            }
            catch (Exception ex)
            {
                msg = $"GetKeyValueFromRedisAsync:key={key} | {ex.Message}";
                _logger.Error(msg);
            }
            return new Response<string>()
            {
                Success = false,
                Message = String.IsNullOrEmpty(msg) ? $"Can't get Redis DB {redisEndPoints}" : msg
            };
        }

        public static async Task<Response<bool>> DeleteFromRedis(string redisEndPoints, string key)
        {
            var msg = string.Empty;
            try
            {
                var database = GetRedisDatabase(redisEndPoints);
                if (database != null)
                {
                    return new Response<bool>()
                    {
                        Success = true,
                        Data = await database.KeyDeleteAsync(key, CommandFlags.FireAndForget)
                    };
                }
            }
            catch (Exception ex)
            {
                msg = $"DeleteFromRedis: key={key} | {ex.Message}";
                _logger.Error(msg);
            }
            return new Response<bool>()
            {
                Success = false,
                Message = String.IsNullOrEmpty(msg) ? $"Can't get Redis DB {redisEndPoints}" : msg
            };
        }

        public static async Task<Response<bool>> KeyExistInRedisAsync(string redisEndPoints, string key)
        {
            var msg = string.Empty;
            try
            {
                var database = GetRedisDatabase(redisEndPoints);
                if (database != null)
                {
                    return new Response<bool>()
                    {
                        Success = true,
                        Data = await database.KeyExistsAsync(key, CommandFlags.PreferSlave)
                    };
                }
            }
            catch (Exception ex)
            {
                msg = $"KeyExistInRedisAsync: key={key} | {ex.Message}";
                _logger.Error(msg);

            }
            return new Response<bool>()
            {
                Success = false,
                Message = String.IsNullOrEmpty(msg) ? $"Can't get Redis DB {redisEndPoints}" : msg
            };
        }

        public sealed class RedisConnection
        {
            private static Lazy<RedisConnection> lazy = new Lazy<RedisConnection>(() =>
            {
                if (String.IsNullOrEmpty(_settingOption)) throw new InvalidOperationException("You need Init() first.");
                return new RedisConnection();
            });

            private static string _settingOption;

            private ConnectionMultiplexer _multiplexer;

            public ConnectionMultiplexer ConnectionMultiplexer
            {
                get
                {
                    if (!_multiplexer.IsConnected)
                    {
                        lock (_multiplexer)
                        {
                            if (_multiplexer != null)
                            {
                                _multiplexer.Dispose();
                            }

                            var options = ConfigurationOptions.Parse(_settingOption);
                            options.SyncTimeout = int.MaxValue;
                            options.ConnectTimeout = 10000;
                            _multiplexer = ConnectionMultiplexer.Connect(_settingOption);
                            _multiplexer.PreserveAsyncOrder = false;
                        }
                    }

                    return _multiplexer;
                }
            }

            public static RedisConnection Instance
            {
                get
                {
                    return lazy.Value;
                }
            }

            private RedisConnection()
            {
                var options = ConfigurationOptions.Parse(_settingOption);
                options.SyncTimeout = int.MaxValue;
                options.ConnectTimeout = 10000;

                _multiplexer = ConnectionMultiplexer.Connect(options);
                _multiplexer.PreserveAsyncOrder = false;
            }

            public static void Init(string settingOption)
            {
                _settingOption = settingOption;
            }
        }
    }
}