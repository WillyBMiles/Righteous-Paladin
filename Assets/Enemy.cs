using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    public int health;
    Animator animator;
    public bool agro = false;

    public GameObject damagePrefab;
    public GameObject shieldedPrefab;

    public int actualRotation;
    CharacterDirection characterDirection;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        actualPosition = transform.position;

        characterDirection = GetComponent<CharacterDirection>();
        characterDirection.SetDirection(actualRotation);
    }

    public float moveDelay = .4f;
    float currentTurnTimer;

    Vector2 actualPosition;
    public bool isShielded;

    public GameObject shield;

    // Update is called once per frame
    void Update()
    {
        if (currentTurnTimer <= 0f)
        {
            OnMoveDecide();
            currentTurnTimer = moveDelay;
        }

        currentTurnTimer -= Time.deltaTime;
    }

    void OnMoveDecide()
    {
        if (!agro)
        {
            foreach (PlayerMovementGridded pmg in FindObjectsOfType<PlayerMovementGridded>())
            {
                if (Vector2.Distance(transform.position, pmg.transform.position) > 10f)
                    continue;
                if (!Physics2D.Raycast(transform.position, pmg.transform.position - transform.position,
                    Vector2.Distance(transform.position, pmg.transform.position), LayerMask.GetMask("Wall")))
                {
                    agro = true;
                }
            }

        }
        if (agro)
        {
            var result = (Vector3)((Vector2)actualPosition) + Quaternion.Euler(0f, 0f, actualRotation) *
            new Vector2(1, 0);
            var result2 = (Vector3)((Vector2)actualPosition) + Quaternion.Euler(0f, 0f, actualRotation - 90) *
                    new Vector2(1, 0);

            void Move()
            {
                OnMove(actualPosition);
                actualPosition = new Vector2(result.x, result.y);

                transform.DOMove((Vector3)(Vector2)actualPosition, .1f);
            }

            if (Physics2D.OverlapCircle(result, .3f, LayerMask.GetMask("Wall")) ||
                Physics2D.OverlapCircle(result, .3f, LayerMask.GetMask("Enemy")) ||
                Physics2D.OverlapCircle(result, .3f, LayerMask.GetMask("Spike")))
            {
                Turn();
                return;
            }

            if (Random.value < .3f && Physics2D.OverlapCircle(result2, .1f, LayerMask.GetMask("Wall"))){
                Move();
                return;
            }
            var c2D = Physics2D.OverlapCircle(result, .1f, LayerMask.GetMask("Player"));
            if (c2D)
            {
                transform.DOMove((Vector3)(Vector2)(actualPosition + (Vector2)result) / 2f,
        .1f / 2f).onComplete += () =>
                {
                    transform.DOMove((Vector3)(Vector2)(actualPosition),

                    .1f / 2f);
                    c2D.GetComponent<PlayerMovementGridded>().TakeDamage();
                };
                return;
            }

            //50% chance to either walk forward or turn left randomly
            float rand = Random.value;
            if (rand < .25f)
            {
                Turn();
                return;
            }
            if (rand < .5f)
            {
                Move();
                return;
            }
            //ELSE target nearest player
            PlayerMovementGridded target = FindObjectsOfType<PlayerMovementGridded>().Aggregate((pmg1, pmg2) => Vector2.Distance(pmg1.transform.position, transform.position) < Vector2.Distance(pmg2.transform.position, transform.position) ? pmg1 : pmg2);
                
                //25% chance, if walking forward would get you closer do that,
                //       otherwise turn left
            if (Vector2.Distance(result, target.transform.position) < Vector2.Distance(transform.position, target.transform.position))
            {
                if (rand < .75f)
                {
                    Move();
                    return;
                }
                //25% chance, if walking forward would get you closer do that,
                //       BUT if a different direction would get you even closer,
                //   TURN LEFT
                
                if (Vector2.Distance(result2, target.transform.position) < Vector2.Distance(result, target.transform.position))
                {
                    Turn();
                    return;
                }
                Move();
                return;


            }
            else
            {
                Turn();
                return;
            }



        }


    }

    public virtual void OnTurn()
    {

    }

    public virtual void OnMove(Vector2 lastPos)
    {

    }


    void Turn(int numTimes = 1)
    {
        //Instantiate(swooshToTurn, transform.position, Quaternion.Euler(0f, 0f, actualRotation), transform);
        actualRotation += 90 * numTimes;
        characterDirection.SetDirection(actualRotation);
        OnTurn();


        //DOTween.To(() => visualRotation, 
        //    (value) => visualRotation = value, actualRotation, smoothTurnDuration);
    }

    public void DealDamage()
    {
        if (isShielded)
        {
            var g = Instantiate(shieldedPrefab, transform);
            g.transform.SetParent(null);
            return;
        }

        GameObject damage = Instantiate(damagePrefab, transform);
        damage.transform.SetParent(null);

        health -= 1;
        if (health <= 0)
        {
            animator.SetTrigger("Die");
            Invoke(nameof(Destroy), 1f);
            PointsManager.AddPoints(100);
        }
    }

    public void DeShield()
    {
        isShielded = false;
        Destroy(shield);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
