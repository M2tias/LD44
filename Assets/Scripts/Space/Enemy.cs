using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private int dir = 1;
    [SerializeField]
    private ShipTactic tactic = ShipTactic.Fleeing;
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

    private int HP = 3;
    private bool boardable = false;
    private bool doCollisionDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        boardSign.SetActive(false);
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
                if(HP == 1)
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
        }

        if(HP == 0)
        {
            tactic = ShipTactic.Broken;
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

    public void DoDamage(int damage)
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
        return boardSign.transform.position;
    }

    public void SetDockingManager(DockingManager manager)
    {
        dockingManager = manager;
    }

    public EnemyType GetEnemyType()
    {
        return type;
    }
}

public enum ShipTactic
{
    Attacking,
    PassingThrough,
    Fleeing,
    Broken
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