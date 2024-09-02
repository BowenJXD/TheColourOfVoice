using System.Collections;
using UnityEngine;

public class OnEnableCon : ConditionBehaviour
{
    public bool loop;
    
    protected override void Init()
    {
        base.Init();
        StartCoroutine(loop ? Exe() : Execute());
    }

    private IEnumerator Exe()
    {
        while (Application.isPlaying)
        {
            yield return Execute();
        }
    }
}