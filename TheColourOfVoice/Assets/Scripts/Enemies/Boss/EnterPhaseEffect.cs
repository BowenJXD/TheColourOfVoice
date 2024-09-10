using UnityEngine;

public class EnterPhaseEffect : ConditionBehaviour
{
    public Health bossHealth;
    public ConditionBehaviour bossBehaviour;
    
    public void EnterPhase(int phase)
    {
        bossHealth.invincible = true;
        bossBehaviour.StopAllCoroutines();
        OnFinish = () =>
        {
            bossBehaviour.StartCoroutine(bossBehaviour.Execute());
            bossHealth.invincible = false;
        };
        StartCoroutine(Execute());
    }
}