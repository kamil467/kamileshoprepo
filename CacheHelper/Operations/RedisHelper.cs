using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CacheHelper.Operations
{
    public class RedisHelper<TKey, TValue> : IRedisHelper<TKey, TValue> where TValue: class? // telling TValue is a nullable type like class or struct
    {
        private readonly IDatabase _database;
        private readonly string _redisKey;

        public RedisHelper(string redisKey,IDatabase database)
        {
            this._redisKey = redisKey;
            this._database = database;
        }

        public TValue this[TKey key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(TKey key, TValue value, TimeSpan? tSpan = null)
        {
           //this._database.HashSet(this._redisKey,key.ToString(),
                 // JsonSerializer.Serialize<TValue>(value));

            this._database.StringSet(this._redisKey + key.ToString(),
                             JsonSerializer.Serialize<TValue>(value),
                             tSpan.Value
                             );
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public TValue GetItem(TKey key)
        {
         //   var jsonSerializeOption = new JsonSerializerOptions
           // {
             //   IgnoreNullValues = true,
                
            //};

            var redisValue = this._database.StringGet(this._redisKey + key.ToString()).ToString();

            if(!string.IsNullOrEmpty(redisValue))
            {
                return JsonSerializer.Deserialize<TValue>(redisValue);
            }

            return null;
        }

        public IEnumerable<TValue> GetValues(string pattern)
        {
            throw new NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
