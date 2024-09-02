using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomSelectExe : ExecutableBehaviour
{
    public bool avoidLastExe;
    public List<ExecutableBehaviour> executables;

    [ReadOnly] public ExecutableBehaviour selectedExe;
    
    protected override void OnStart()
    {
        base.OnStart();
        var tmp = new List<ExecutableBehaviour>(executables);
        if (avoidLastExe && selectedExe && executables.Count > 1)
        {
            tmp.Remove(selectedExe);
        }
        selectedExe = tmp[Random.Range(0, tmp.Count)];
    }

    protected override IEnumerator Executing()
    {
        return selectedExe.Execute(blackboard);
    }
}