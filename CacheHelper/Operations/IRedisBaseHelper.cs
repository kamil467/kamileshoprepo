using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheHelper.Operations
{
    public interface IRedisBaseHelper<TKey,TValue>
    {
        IRedisHelper<TKey, TValue> GetRedisHelper(string key,int db);
    }
}
