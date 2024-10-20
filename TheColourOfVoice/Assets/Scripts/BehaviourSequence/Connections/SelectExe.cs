using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class SelectExe : ExecutableBehaviour
{
    public List<ExecutableBehaviour> executables;

    [ReadOnly] public ExecutableBehaviour selectedExe;

    public abstract void Select();
    
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
        Select();
    }

    protected override IEnumerator Executing()
    {
        return selectedExe?.Execute(blackboard);
    }
}