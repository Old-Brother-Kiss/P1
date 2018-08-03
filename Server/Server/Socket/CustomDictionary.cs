using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CustomDictionary<Key, Value>
{
    private Dictionary<Key, Value> key_to_value = new Dictionary<Key, Value>();
    private Dictionary<Value, Key> value_to_key = new Dictionary<Value, Key>();

    public void Add(Key key, Value value)
    {
        this.key_to_value[key] = value;
        this.value_to_key[value] = key;
    }

    public void Remove(Key key)
    {
        Value value = this.key_to_value[key];
        this.key_to_value.Remove(key);
        this.value_to_key.Remove(value);
    }

    public void Remove(Value value)
    {
        Key key = this.value_to_key[value];
        this.value_to_key.Remove(value);
        this.key_to_value.Remove(key);
    }

    public bool HaveKey(Key key)
    {
        return this.key_to_value.ContainsKey(key);
    }

    public bool HaveValue(Value value)
    {
        return this.value_to_key.ContainsKey(value);
    }

    public Key GetKey(Value value)
    {
        return this.value_to_key[value];
    }

    public Value GetValue(Key key)
    {
        return this.key_to_value[key];
    }
}
