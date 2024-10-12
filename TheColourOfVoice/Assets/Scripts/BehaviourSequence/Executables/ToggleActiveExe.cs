using System.Collections.Generic;
using UnityEngine;

public class ToggleActiveExe : ExecutableBehaviour
{
    public bool activeOrInactive = true;
    
    public List<GameObject> targets = new List<GameObject>();
    
    protected override void OnStart()
    {
        base.OnStart();
        foreach (var target in targets)
        {
            target.SetActive(activeOrInactive);
        }
    }
}