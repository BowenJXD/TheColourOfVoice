using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Entity
{
/*    [SerializeField] bool showOnHealthBar = true;

    [SerializeField] StatsBar onHeadHealthBar;
*/
    public override void SetUp()
    {
        base.SetUp();
        Health health = GetComponent<Health>();
        health.OnDeath += Deinit;
    }

/*    protected virtual void OnEnable()
    {
        health = maxHealth;
        if (showOnHealthBar)
        {
            ShowOnHeadHealthBar();

        }
        else
        {
            HideOnHealthBar();
        }
    }


    public void ShowOnHeadHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(true);
        onHeadHealthBar.Initialize(health, maxHealth);

    }

    public void HideOnHealthBar()
    {
        onHeadHealthBar.gameObject.SetActive(false);
    }*/

    //take damage
}