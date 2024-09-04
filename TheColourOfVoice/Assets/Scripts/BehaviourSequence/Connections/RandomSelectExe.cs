using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomSelectExe : ExecutableBehaviour
{
    public bool avoidLastExe;
    public List<ExecutableBehaviour> executables;

    [ReadOnly] public ExecutableBehaviour selectedExe;

    public override void Init()
    {
        base.Init();
        foreach (var exe in executables)
        {
            exe.Init();
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (avoidLastExe && !selectedExe && executables.Count > 0)
        {
            selectedExe = executables[^1];
            return;
        }
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