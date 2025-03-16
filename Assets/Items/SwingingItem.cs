using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingItem : Item
{
    public Animator animator;
    public SoundRandomizer source;

    protected override void InternalUse()
    {
        source.Play();
        animator.SetTrigger("Use");
        animator.speed = PointsManager.SpeedMult();
        Invoke(nameof(Hit), .2f * PointsManager.SpeedMult());
    }
    void Hit()
    {
        if (playerMovement == null)
            return;
        if (playerMovement.lastTimeAttacked + cooldown > Time.time)
            return;

        var e = playerMovement.ObjectsImmediatelyInFront(.4f);
        if (e == null)
            return;
        if (e.TryGetComponent(out Enemy enemy))
        {
            enemy.DealDamage();
            playerMovement.lastTimeAttacked = Time.time;
        }
    }
}
