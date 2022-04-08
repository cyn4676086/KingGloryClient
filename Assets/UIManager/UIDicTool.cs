using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIDicTool{
    public static TValue TryGetValueByNN<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey type )
    {
        TValue v;

        dic.TryGetValue(type, out v);

        return v;
    }
    
}
