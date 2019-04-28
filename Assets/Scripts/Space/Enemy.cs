using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Collider2D enemyCollider;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private int dir = 1;
    [SerializeField]
    private ShipTactic tactic = ShipTactic.Fleeing;
    private ShipTactic previousTactic = ShipTactic.Fleeing;
    private AttackPhase attackPhase = AttackPhase.Attacking;
    [SerializeField]
    private PlayerShipRuntime playerShipRuntime;
    [SerializeField]
    private EnemyHPMeter hpMeter;
    [SerializeField]
    private GameObject boardSign;
    [SerializeField]
    private DockingManager dockingManager;
    [SerializeField]
    private EnemyType type;
    [SerializeField]
    private Astronaut astronautPrefab;
    [SerializeField]
    private List<GameObject> explosionParts;

    //ShipTactic.Attacking
    private float attackChaseTime = 2.5f;
    private float attackChaseStarted = 0f;
    private float halfShootRange = 5f;
    private float shootRange = 3f; // how far away will it shoot at the player
    private float backoutXRange = 1.5f;
    private float chaseFailedBackoutTime = 1f;
    private float chaseFailedBackoutStarted = 0f;
    private float hitLandedBackoutTime = 1f; // maybe just same as the invulnerability of the player
    private float hitLandedBackoutStarted = 0f;
    private float hitLandedStartY = 0f;
    private bool shooting = false;
    //ShipTactic.Fleeing
    private float fleeingStartY = float.NaN;
    private float fleeingYMax = 1.5f;
    private float fleeingYMin = -1.5f;
    private float fleeingMoveY = 1;
    private float fleeingMoveYCoef = 1.5f;

    [SerializeField]
    private float HP = 3;
    private bool boardable = false;
    private bool doCollisionDamage = true;
    private bool spawnedAstronaut = false;

    private float lastHealed = 0f;
    private float healFrequency = 10f;
    private float healTillGood = 3f;

    private float maxY { get { return 7.5f; } }
    private float minY { get { return -5f; } }

    // Start is called before the first frame update
    void Start()
    {
        boardSign.SetActive(false);
        enemyCollider = transform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        int moveX = 1;
        float moveY = 0;
        if (tactic == ShipTactic.Attacking)
        {

            if (attackPhase == AttackPhase.Attacking)
            {
                shooting = true;
                dir = transform.position.x > playerShipRuntime.Position.x ? -1 : 1;
                if(HP == 1 && type != EnemyType.Fighter)
                {
                    tactic = ShipTactic.Fleeing;
                }
                //ship isn't in the shooting range
                if (Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) > halfShootRange)
                {
                    moveX = 1;
                    moveY = 0;
                }
                else if (Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) <= halfShootRange && Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) > shootRange)
                {
                    moveY = playerShipRuntime.Position.y > transform.position.y ? 1 : -1;
                    moveX = 1;
                }
                else if (Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) < backoutXRange)
                {
                    moveX = 1;
                    moveY = 0;
                    dir = transform.position.x > playerShipRuntime.Position.x ? 1 : -1;
                }
                else
                {
                    if (Mathf.Abs(playerShipRuntime.Position.y - (transform.position.y - 0.1f)) > 0.1f)
                    {
                        moveY = playerShipRuntime.Position.y > transform.position.y ? 1 : -1;
                    }
                    else
                    {
                        moveY = 0;
                    }
                    moveX = 0;
                }
                if (playerShipRuntime.Invulnerable == true)
                {
                    attackPhase = AttackPhase.HitLanded;
                    hitLandedBackoutStarted = Time.time;
                    hitLandedStartY = transform.position.y;
                }
                else if (attackChaseStarted + attackChaseTime < Time.time)
                {
                    attackPhase = AttackPhase.BackingOut;
                    chaseFailedBackoutStarted = Time.time;
                }
            }
            else if (attackPhase == AttackPhase.HitLanded)
            {
                shooting = false;
                //ship isn't in the shooting range
                if (Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) > shootRange)
                {
                    moveX = 1;
                    moveY = 0;
                }
                else if (Mathf.Abs(transform.position.x - playerShipRuntime.Position.x) < backoutXRange)
                {
                    moveX = 1;
                    moveY = 0;
                    dir = transform.position.x > playerShipRuntime.Position.x ? 1 : -1;
                }
                moveY = hitLandedStartY > 0 ? -1 : 1;
                if (hitLandedBackoutStarted + hitLandedBackoutTime < Time.time)
                {
                    attackPhase = AttackPhase.Attacking;
                    attackChaseStarted = Time.time;
                }
            }
            else
            {
                shooting = false;
                moveX = 0;
                moveY = 0;
                if (chaseFailedBackoutStarted + chaseFailedBackoutTime < Time.time)
                {
                    attackPhase = AttackPhase.Attacking;
                    attackChaseStarted = Time.time;
                }
            }
        }
        else if(tactic == ShipTactic.PassingThrough)
        {
            if ((dir > 0 && playerShipRuntime.Position.x > transform.position.x) || (dir < 0 && playerShipRuntime.Position.x < transform.position.x))
            {
                if (Mathf.Abs(playerShipRuntime.Position.y - transform.position.y) < 1f && playerShipRuntime.Position.y >= transform.position.y)
                {
                    moveY = -1;
                }
                if (Mathf.Abs(playerShipRuntime.Position.y - transform.position.y) < 1f && playerShipRuntime.Position.y < transform.position.y)
                {
                    moveY = 1;
                }
            }
            if(HP == 1)
            {
                tactic = ShipTactic.Fleeing;
            }
        }
        else if(tactic == ShipTactic.Fleeing)
        {
            if(float.IsNaN(fleeingStartY))
            {
                fleeingStartY = transform.position.y;
            }

            if (fleeingStartY + fleeingYMax < transform.position.y)
            {
                fleeingMoveY = -1 * fleeingMoveYCoef;
            }
            else if (fleeingStartY + fleeingYMin > transform.position.y)
            {
                fleeingMoveY = 1 * fleeingMoveYCoef;
            }

            if(transform.position.x > playerShipRuntime.Position.x)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }

            //military ships heal on fleeing mode
            if (type == EnemyType.Military)
            {
                if (lastHealed == 0)
                {
                    lastHealed = Time.time-5f;
                }

                if(Time.time > lastHealed + healFrequency)
                {
                    HP += 1;
                    lastHealed = Time.time;
                }

                if(HP >= healTillGood)
                {
                    tactic = ShipTactic.Attacking;
                    lastHealed = 0;
                }
            }

            moveY = fleeingMoveY;
            shooting = false;
        }
        else if(tactic == ShipTactic.Broken)
        {
            moveX = 0;
            moveY = 0;
            boardable = true;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            doCollisionDamage = false;
            hpMeter.gameObject.SetActive(false);
            boardSign.SetActive(true);
            dockingManager.SetDockable(this);
            shooting = false;
        }
        else if(tactic == ShipTactic.Explode)
        {
            foreach(GameObject explObj in explosionParts)
            {
                explObj.SetActive(true);
            }

            foreach(Transform child in transform)
            {
                SpriteRenderer renderer = child.GetComponent<SpriteRenderer>();
                if(renderer != null)
                {
                    renderer.enabled = false;
                }
            }

            if(type == EnemyType.Civilian && !spawnedAstronaut)
            {
                spawnedAstronaut = true;
                for(var i = 0; i < 3; i++)
                {
                    Astronaut astronaut = Instantiate(astronautPrefab);
                    Vector3 v = Random.onUnitSphere;
                    Vector3 v2 = Vector3.Normalize(new Vector3(v.x, v.y, 0));
                    Vector2 x = new Vector2(v2.x, v2.y);
                    astronaut.transform.position = transform.position;
                    astronaut.SetDirection(x);
                }
            }

            doCollisionDamage = false;
            enemyCollider.enabled = false;
            shooting = false;
            hpMeter.gameObject.SetActive(false);
            boardSign.SetActive(false);
            moveX = 0;
            moveY = 0;
            StartCoroutine(WaitAndDestroy());
        }
        else if(tactic == ShipTactic.TooFar)
        {
            if(playerShipRuntime.Position.x >= transform.position.x)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }

            if ((dir > 0 && playerShipRuntime.Position.x > transform.position.x) || (dir < 0 && playerShipRuntime.Position.x < transform.position.x))
            {
                if (Mathf.Abs(playerShipRuntime.Position.y - transform.position.y) < 1f && playerShipRuntime.Position.y >= transform.position.y)
                {
                    moveY = -1;
                }
                if (Mathf.Abs(playerShipRuntime.Position.y - transform.position.y) < 1f && playerShipRuntime.Position.y < transform.position.y)
                {
                    moveY = 1;
                }
            }

            if(Mathf.Abs(playerShipRuntime.Position.x - transform.position.x) < 10)
            {
                tactic = previousTactic;
            }
        }

        if(HP == 0)
        {
            if (type == EnemyType.Cargo)
            {
                tactic = ShipTactic.Broken;
            }
            else
            {
                tactic = ShipTactic.Explode;
            }
        }

        if(tactic != ShipTactic.Explode && tactic != ShipTactic.Broken && tactic != ShipTactic.TooFar)
        {
            if(Vector3.Distance(playerShipRuntime.Position, transform.position) > 40)
            {
                previousTactic = tactic;
                tactic = ShipTactic.TooFar;
            }
        }

        if(transform.position.y > maxY && moveY > 0)
        {
            moveY = 0;
        }
        else if(transform.position.y < minY && moveY < 0)
        {
            moveY = 0;
        }

        body.velocity = new Vector2(velocity.x * Time.deltaTime * dir * moveX, velocity.y * Time.deltaTime * moveY);

        hpMeter.SetHealth(HP);
    }

    public int GetDir()
    {
        return dir;
    }

    public bool GetShooting()
    {
        return shooting;
    }

    public void DoDamage(float damage)
    {
        HP -= damage;
    }

    public bool GetBoardable()
    {
        return boardable;
    }

    public bool GetCollisionDamage()
    {
        return doCollisionDamage;
    }

    public Vector3 GetDockingPosition()
    {
        return boardSign.transform.position + Vector3.down * 0.2f;
    }

    public void SetDockingManager(DockingManager manager)
    {
        dockingManager = manager;
    }

    public EnemyType GetEnemyType()
    {
        return type;
    }

    IEnumerator WaitAndStartExplosion()
    {
        yield return new WaitForSeconds(3);
        tactic = ShipTactic.Explode;
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void StopDock()
    {
        StartCoroutine(WaitAndStartExplosion());
    }
}

public enum ShipTactic
{
    Attacking,
    PassingThrough,
    Fleeing,
    Broken,
    Explode,
    TooFar
}

public enum AttackPhase
{
    Attacking,
    BackingOut,
    HitLanded
}

public enum EnemyType
{
    Military,
    Civilian,
    Cargo,
    Fighter
}