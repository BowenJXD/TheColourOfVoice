using System.Collections.Generic;
using UnityEngine;

public class ToggleEnabledExe : ExecutableBehaviour
{
    public bool enableOrDisable = true;
    
    public List<Behaviour> components;

    protected override void OnStart()
    {
        base.OnStart();
        components.ForEach(c => c.enabled = enableOrDisable);
    }
}