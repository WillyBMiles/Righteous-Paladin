using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [HideInInspector]
    public bool onGround = false;
    [HideInInspector]
    public Inventory inventory;

    public float cooldown;

    float currentCooldown = 0f;
    [HideInInspector]
    public float pickupCooldown = 0f;

    public GameObject groundSprite;
    public GameObject handSprite;
    public Sprite pocketSprite;

    public PlayerMovementGridded playerMovement;

    [HideInInspector]
    public Collider2D c2D;

    public AudioClip useSound;

    private void Awake()
    {
        c2D = GetComponent<Collider2D>();
        groundSprite.SetActive(true);
        handSprite.SetActive(false);
    }

    private void Update()
    {
        pickupCooldown -= Time.deltaTime;
        currentCooldown -= Time.deltaTime;
    }

    public virtual void Use()
    {
        if (currentCooldown <= 0f)
        {

            InternalUse();
            currentCooldown = cooldown * PointsManager.SpeedMult();

        }
           
    }
    protected void PlaySound()
    {
        if (useSound)
            AudioSource.PlayClipAtPoint(useSound, transform.position);
    }

    public void SetPickupCooldown()
    {
        pickupCooldown = .5f;
    }


    protected abstract void InternalUse();

    public virtual void Pickup()
    {
        //pass
    }

    public virtual void Drop()
    {
        //pass
    }
}
