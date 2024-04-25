using UnityEngine;
using UnityEngine.Pool;

public class FreezeBuff : Buff
{
    private Rigidbody2D rb;
    private Attack attack;
    RigidbodyConstraints2D cachedConstraints;

    public override void OnApply(BuffOwner buffOwner)
    {
        base.OnApply(buffOwner);
        if (buffOwner.TryGetComponent(out Rigidbody2D rb))
        {
            this.rb = rb;
            cachedConstraints = rb.constraints;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        if (buffOwner.TryGetComponentInChildren(out Attack attack))
        {
            this.attack = attack;
            attack.gameObject.SetActive(false);
        }
    }
    
    public override void OnRemove()
    {
        base.OnRemove();
        if (rb) rb.constraints = cachedConstraints;
        if (attack) attack.gameObject.SetActive(true);
    }
}