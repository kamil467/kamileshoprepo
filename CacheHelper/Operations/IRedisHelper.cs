using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CacheHelper.Operations
{
    public interface IRedisHelper<TKey, TValue> :IDictionary<TKey, TValue>
    {
        public void Add(TKey key, TValue value, TimeSpan? tSpan=null);

        public TValue GetItem(TKey key);

        public bool Remove(TKey key);

        public IEnumerable<TValue> GetValues(string pattern);
    }
}
