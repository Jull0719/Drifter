using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<Tkey, Tval> : Dictionary<Tkey, Tval>, ISerializationCallbackReceiver
{
    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<Tval> values = new List<Tval>();

    // 反序列化之后调用
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            Debug.Log("键和值的数目不匹配");

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }

    // 序列化之前调用
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach (var kv in this)
        {
            keys.Add(kv.Key);
            values.Add(kv.Value);
        }
    }
}
