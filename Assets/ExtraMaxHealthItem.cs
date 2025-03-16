using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMaxHealthItem : Item
{
    protected override void InternalUse()
    {
        //pass
    }

    public override void Pickup()
    {
        base.Pickup();
        playerMovement.maxHealth += 1;
        playerMovement.Heal();
    }
    public override void Drop()
    {
        base.Drop();
        playerMovement.maxHealth -= 1;
        while (playerMovement.Health > playerMovement.maxHealth)
        {
            playerMovement.TakeDamage();
        }
    }
}
