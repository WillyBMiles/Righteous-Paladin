using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingItem : Item
{
    protected override void InternalUse()
    {
        playerMovement.Heal();
    }

    bool fullHealth = true;
    float internalCd = -1f;
    public override void Use()
    {
        base.Use();
        if (internalCd <= 0f && !fullHealth)
        {
            playerMovement.Heal();
            PlaySound();
            fullHealth = playerMovement.Health == playerMovement.maxHealth;
            internalCd = cooldown;
        }

        if (fullHealth && playerMovement.Health < playerMovement.maxHealth)
        {
            internalCd = cooldown;
        }

        internalCd -= Time.deltaTime;
    }

    public override void Pickup()
    {
        base.Pickup();
        internalCd = cooldown;
        fullHealth = true;
    }
}
