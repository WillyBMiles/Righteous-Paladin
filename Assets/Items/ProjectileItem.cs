using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileItem : Item
{
    public Projectile projectilePrefab;

    protected override void InternalUse()
    {
        PlaySound();
        Projectile p = Instantiate(projectilePrefab, transform.position, transform.rotation
             );
        
    }
}
