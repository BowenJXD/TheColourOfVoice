using System.Collections;
using System.Collections.Generic;

public class SequenceExe : ExecutableBehaviour
{
    public List<ExecutableBehaviour> executables;

    protected override IEnumerator Executing()
    {
        foreach (var exe in executables)
        {
            yield return exe.Execute(blackboard);
        }
    }
}