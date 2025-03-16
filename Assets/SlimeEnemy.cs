using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    public GameObject drop;

    public override void OnMove(Vector2 lastPos)
    {
        base.OnMove(lastPos);
        Instantiate(drop, lastPos, Quaternion.identity);
    }
}
