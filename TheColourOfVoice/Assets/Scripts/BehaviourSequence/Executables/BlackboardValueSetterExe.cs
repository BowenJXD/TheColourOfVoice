using System.Collections.Generic;

public class BlackboardValueSetterExe : ExecutableBehaviour
{
    public SerializableDictionary<string, object> values;
    
    protected override void OnStart()
    {
        foreach (var value in values)
        {
            blackboard.Set(value.Key, value.Value);
        }
    }
}