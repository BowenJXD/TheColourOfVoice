using System.Collections;
using UnityEngine;

public class OnEnableCon : ConditionBehaviour
{
    public bool loop;
    
    protected override void Init()
    {
        base.Init();
        StartCoroutine(Execute());
    }

    public override IEnumerator Execute()
    {
        do
        {
            yield return base.Execute();
        }
        while (loop && Application.isPlaying);
    }
}