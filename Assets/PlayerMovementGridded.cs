using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementGridded : MonoBehaviour
{
    public KeyCode key;
    public float moveDelay;
    public float smoothTurnDuration;
    public float smoothMoveDuration;

    public int maxHealth;

    public Vector2 ActualPosition => actualPosition;
    int health;
    public int Health => health;

    public System.Action OnCollision;

    CharacterDirection characterDirection;

    public GameObject swooshToTurn;

    public GameObject damagePrefab;
    public GameObject healPrefab;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        actualPosition = transform.position;
        characterDirection = GetComponent<CharacterDirection>();
        characterDirection.SetDirection(actualRotation);
    }
    float currentTurnTimer;
    public int actualRotation;

    //float visualRotation;
    Vector2 actualPosition;

    bool shouldMove;
    // Update is called once per frame
    void Update()
    {
        // Input.GetKeyDown(key)
        if ( Input.anyKeyDown)
        {
            Turn();
            shouldMove = true;
        }
        // Input.GetKeyUp(key)
        if (Input.anyKey)
        {
            currentTurnTimer = moveDelay ;
        }

        currentTurnTimer -= Time.deltaTime;
        //!Input.GetKey(key) 
        if (shouldMove && !Input.anyKey && currentTurnTimer <= 0)
        {
            Move();
            currentTurnTimer = moveDelay * PointsManager.SpeedMult();

        }
        damageTime -= Time.deltaTime;


        //transform.rotation = Quaternion.Euler(0f, 0f, visualRotation);

        //if (Mathf.Abs(actualRotation - visualRotation) < .01f)
        //{
        //    actualRotation %= 360;
        //    visualRotation = actualRotation;
        //}
    }
    public float lastTimeAttacked;
    void Move(bool secondTime = false)
    {
        var result = (Vector3)((Vector2)actualPosition) + Quaternion.Euler(0f, 0f, actualRotation) *
            new Vector2(1, 0);

        if (Physics2D.OverlapCircle(result, .1f, LayerMask.GetMask("Wall")))
        {
            transform.DOMove((Vector3)(Vector2)(actualPosition + (Vector2)result) / 2f,
                smoothMoveDuration / 2f).onComplete += () =>
                {
                    transform.DOMove((Vector3)(Vector2)(actualPosition),
                    smoothMoveDuration / 2f);
                    //Turn();
                    shouldMove = false;
                    OnCollision();
                };
            
            
            //if (!secondTime)
            //    Move(true);
            return;

        }
        Collider2D c2D = ObjectsImmediatelyInFront();
        if (c2D != null && c2D.GetComponent<Enemy>()){ // hit an enemy
            transform.DOMove((Vector3)(Vector2)(actualPosition + (Vector2)result) / 2f,
                smoothMoveDuration / 2f).onComplete += () => 
                { 
                    transform.DOMove((Vector3)(Vector2)(actualPosition),
                        smoothMoveDuration / 2f);
                    Inventory i = GetComponent<Inventory>();

                    if (i.Items.Count > 0 && i.Items[0] is SwingingItem)
                    {
                        if (lastTimeAttacked + .5f <= Time.time)
                        {
                            c2D.GetComponent<Enemy>().DealDamage();
                            lastTimeAttacked = Time.time;
                        }
                        
                    }
                    else 
                        TakeDamage();
                
                };

            return;
        }

        actualPosition = new Vector2(result.x, result.y);

        transform.DOMove((Vector3)(Vector2)actualPosition, smoothMoveDuration);
    }
    float damageTime = 1f;
    void Turn(int numTimes = 1)
    {
        Instantiate(swooshToTurn, transform.position, Quaternion.Euler(0f, 0f, actualRotation), transform);
        actualRotation -= 90 * numTimes;
        characterDirection.SetDirection(actualRotation);
        while (actualRotation < 0)
        {
            actualRotation += 360;
        }

        

        //DOTween.To(() => visualRotation, 
        //    (value) => visualRotation = value, actualRotation, smoothTurnDuration);
    }

    public void TakeDamage()
    {

        if (damageTime > 0f)
            return;
        damageTime = 1f;
        
        Instantiate(damagePrefab, transform);

        if (health > 1)
        {
            health -= 1;
        }
        else
        {
            GetComponent<Inventory>().DropItem(1);
            PointsManager.AddPoints(-100);
        }
        
        //if (health <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    public Collider2D ObjectsImmediatelyInFront(float size = .25f)
    {
        var result = (Vector3)((Vector2)actualPosition) + Quaternion.Euler(0f, 0f, actualRotation) *
    new Vector2(1, 0);
        List<Collider2D> results = new();
        Collider2D c2D = Physics2D.OverlapCircle(result, size);
        return c2D;
    }

    public void Heal()
    {
        if (health < maxHealth)
        {
            Instantiate(healPrefab, transform);
        }
        health += 1;
        if (health > maxHealth)
            health = maxHealth;
    }
}
