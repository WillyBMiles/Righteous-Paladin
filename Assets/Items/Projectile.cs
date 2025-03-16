using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb2D;

    public bool hitsFriendlies = false;
    public bool destroyesShields = false;

    public AudioClip onHit;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.velocity = transform.right * speed * PointsManager.SpeedMult();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 7)
        {
            if (hitsFriendlies)
                return;
            Enemy e = collision.GetComponent<Enemy>();
            if (destroyesShields)
            {
                e.DeShield();
                AudioSource.PlayClipAtPoint(onHit, transform.position);
            }
            else
            e.DealDamage();
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 3)
        {
            if (!hitsFriendlies)
                return;
            PlayerMovementGridded pmg = collision.GetComponent<PlayerMovementGridded>();

            pmg.TakeDamage();
            Destroy(gameObject);
        }
    }
}
