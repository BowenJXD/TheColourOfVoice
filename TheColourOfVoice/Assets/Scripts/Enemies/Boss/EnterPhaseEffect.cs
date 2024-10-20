using UnityEngine;

public class EnterPhaseEffect : ConditionBehaviour
{
    public Health bossHealth;
    public ConditionBehaviour bossBehaviour;
    
    public void EnterPhase(bool stop)
    {
        bossHealth.invincible = true;
        bossBehaviour.StopAllCoroutines();
        OnFinish = () =>
        {
            if (!stop) bossBehaviour.StartCoroutine(bossBehaviour.Execute());
            bossHealth.invincible = false;
        };
        StartCoroutine(Execute());
    }
}