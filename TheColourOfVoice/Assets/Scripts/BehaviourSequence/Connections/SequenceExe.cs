using System.Collections;
using System.Collections.Generic;

public class SequenceExe : ExecutableBehaviour
{
    public int loop = 1;
    
    public List<ExecutableBehaviour> executables;

    public override void Init()
    {
        base.Init();
        if (executables.Count == 0)
        {
            executables = new List<ExecutableBehaviour>(GetComponents<ExecutableBehaviour>());
            executables.Remove(this);
        }
        foreach (var exe in executables)
        {
            exe.Init();
        }
    }

    protected override IEnumerator Executing()
    {
        for (int i = 0; i < loop || loop == 0; i++)
        {
            foreach (var exe in executables)
            {
                yield return exe.Execute(blackboard);
            }
        }
    }
}