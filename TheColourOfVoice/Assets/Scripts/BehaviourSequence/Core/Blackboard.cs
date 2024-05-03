using System;
using System.Collections.Generic;

public class Blackboard
{
    public Dictionary<string, object> blackboard;
    
    public Blackboard()
    {
        blackboard = new();
    }

    public void Set(string key, object value)
    {
        
        blackboard[key] = value;
    }
        
    public T Get<T>(string key)
    {
        if (!blackboard.ContainsKey(key))
        {
            return default;
        }
        return (T) blackboard[key];
    }
        
    public bool TryGet<T>(string key, out T value)
    {
        if (blackboard == null || !blackboard.ContainsKey(key))
        {
            value = default;
            return false;
        }
            
        if (blackboard[key] is T)
        {
            value = (T) blackboard[key];
            return true;
        }
            
        try
        {
            value = (T)Convert.ChangeType(blackboard[key], typeof(T));
            return true;
        }
        catch (InvalidCastException)
        {
            value = default;
        }
        catch (FormatException)
        {
            value = default;
        }
        return false;
    }
}