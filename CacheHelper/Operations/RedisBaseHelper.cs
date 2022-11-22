using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheHelper.Operations
{
    /// <summary>
    /// Factory Pattern
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class RedisBaseHelper<TKey, TValue> : IRedisBaseHelper<TKey, TValue> where TValue:class
    {
        private readonly IConnectionMultiplexer connectionMultiplexer;

        private IDatabase database;

        public RedisBaseHelper(IConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        public IRedisHelper<TKey, TValue> GetRedisHelper(string key,int db)
        {
            IRedisHelper<TKey, TValue> redisHelper = null;
            switch (db)
             {
                case 0:
                    this.database = this.connectionMultiplexer.GetDatabase(db);
                    redisHelper =  new RedisHelper<TKey, TValue>(key, this.database);
                    break;
                case 1:
                    this.database = this.connectionMultiplexer.GetDatabase(db);
                    redisHelper = new RedisHelper<TKey, TValue>(key, this.database);
                    break;
                case 2:
                    this.database = this.connectionMultiplexer.GetDatabase(db);
                    redisHelper = new RedisHelper<TKey, TValue>(key, this.database);
                    break;
                case 3:
                    this.database = this.connectionMultiplexer.GetDatabase(db);
                    redisHelper = new RedisHelper<TKey, TValue>(key, this.database);
                    break;
                default:
                    this.database = this.connectionMultiplexer.GetDatabase(db);
                    redisHelper = new RedisHelper<TKey, TValue>(key, this.database);
                    break;

            }

            return redisHelper;


        }
    }
}
