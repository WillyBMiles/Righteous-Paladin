using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : Item
{
    protected override void InternalUse()
    {
       
        bool used = false;
        var collider = playerMovement.ObjectsImmediatelyInFront();
        if (collider == null)
            return;
        if (collider.TryGetComponent<Door>(out Door door))
        {
            Destroy(door.gameObject);
            used = true;

        }

        if (used)
        {
            PlaySound();
            playerMovement.GetComponent<Inventory>().DropItem(0, false);
            Destroy(gameObject);
            PointsManager.AddPoints(100);
        }
    }
}
