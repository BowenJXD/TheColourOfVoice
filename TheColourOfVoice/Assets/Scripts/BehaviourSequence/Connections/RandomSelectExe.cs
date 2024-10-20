using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandomSelectExe : SelectExe
{
    public bool avoidLastExe;

    public override void Select()
    {
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
}