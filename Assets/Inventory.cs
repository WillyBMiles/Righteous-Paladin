using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    List<Item> items = new();
    public IReadOnlyList<Item> Items => items;

    protected PlayerMovementGridded pmg;

    public AudioClip drop;
    public AudioClip pickup;
    // Start is called before the first frame update
    virtual protected void Start()
    {
        pmg = GetComponent<PlayerMovementGridded>();
        pmg.OnCollision += () => DropItem(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (items.Count > 0)
        {
            Item currentItem = items[0];
            currentItem.Use();
            currentItem.groundSprite.SetActive(false);
            currentItem.handSprite.SetActive(true);
            currentItem.transform.rotation = Quaternion.Euler(0f, 0f, pmg.actualRotation);
        }
        
    }

    public void PickupItem(Item item)
    {
        items = items.Prepend(item).ToList();
        item.onGround = false;
        item.transform.parent = transform;
        item.transform.position = transform.position;
        item.transform.rotation = transform.rotation;
        item.playerMovement = pmg;
        item.inventory = this;
        item.groundSprite.SetActive(false);
        item.c2D.enabled = false;
        items.ForEach(item => item.handSprite.SetActive(false));
        item.Pickup();
        AudioSource.PlayClipAtPoint(pickup, transform.position);
    }

    public void DropItem(int range, bool playSound = true)
    {
        if (items.Count == 0)
            return;

        Item item = items[0];

        items.RemoveAt(0);
        item.Drop();
        item.transform.rotation = Quaternion.identity;
        item.onGround = false;
        item.transform.parent = null;

        var dropPos = pmg.ActualPosition;
        if (range > 0)
        {
            var newPos = dropPos + new Vector2(Random.Range(-range, range + 1), Random.Range(-range, range + 1));
            int count = 0;
            while (Physics2D.OverlapCircle(newPos, .1f, LayerMask.GetMask("Wall"))
                || (Physics2D.OverlapCircle(newPos, .1f, LayerMask.GetMask("Spike") )
                && count < 10)){
                count++;
                newPos = dropPos + new Vector2(Random.Range(-range, range + 1), Random.Range(-range, range + 1));
            }
            dropPos = newPos;
        }
        item.transform.position = dropPos;
        item.SetPickupCooldown();
        item.inventory = null;
        item.groundSprite.SetActive(true);
        item.c2D.enabled = true;
        item.handSprite.SetActive(false);
        item.playerMovement = null;
        if (playSound)
        AudioSource.PlayClipAtPoint(drop, transform.position);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Item>(out Item item))
        {
            if (item.pickupCooldown > 0f)
            {
                return;
            }
            PickupItem(item);
        }
    }
}
