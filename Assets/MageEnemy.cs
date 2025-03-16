using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageEnemy : Enemy
{
    public GameObject projectile;
    public override void OnTurn()
    {
        base.OnTurn();
        Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, actualRotation));
    }
}
