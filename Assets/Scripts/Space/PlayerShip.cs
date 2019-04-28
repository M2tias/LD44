using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private Collider2D shipCollider;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private PlayerSpaceAmmo ammoPrefab = null;
    [SerializeField]
    private List<SpriteRenderer> engineEffects;
    [SerializeField]
    private float fireWait = 0.33f;
    [SerializeField]
    private PlayerShipRuntime playerShipRuntime;
    [SerializeField]
    private DockingManager dockingManager;
    [SerializeField]
    private Animator shipAnimator;
    [SerializeField]
    private Astronaut astronautPrefab;

    private bool dockingOngoing = false;
    private Vector3 dockingPosition;
    private Vector3 dockingStart;
    private float dockingStartTime = 0f;
    private float dockingTime = 0f;
    private float dockingDistance = 0f;
    private Enemy dockTarget;

    //invuln
    private float invul_started = 0f;
    private float invul_time = 1f;
    private float lastFlash = 0f;
    private float flashTime = 0.1f;
    private bool flash = false;
    private Shader shaderFlash;
    private Shader shaderSpritesDefault;
    private int flashCount = 0;
    //~invuln

    private float lastFire = 0f;

    private int dir = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerShipRuntime.HP = 10;
        playerShipRuntime.Ammo = 45;
        shaderFlash = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vel = body.velocity;
        doInvuln();

        if (Input.GetKey(KeyCode.W))
        {
            vel = new Vector2(vel.x, velocity.y * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vel = new Vector2(vel.x, -velocity.y * Time.deltaTime);
        }
        else
        {
            vel = new Vector2(vel.x, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            vel = new Vector2(velocity.x * Time.deltaTime, vel.y);
            dir = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            vel = new Vector2(-velocity.x * Time.deltaTime, vel.y);
            dir = -1;
        }
        else
        {
            vel = new Vector2(0, vel.y);
        }

        if (Input.GetKey(KeyCode.H))
        {
            fire();
        }
        else if(Input.GetKey(KeyCode.J) && !dockingOngoing)
        {
            Enemy closest = null;
            float minD = 100000f;
            foreach(Enemy dockable in dockingManager.GetDockableEnemies())
            {
                float dockableDistance = Vector3.Distance(dockable.transform.position, transform.position);
                if (dockableDistance < minD)
                {
                    minD = dockableDistance;
                    closest = dockable;
                }
            }

            if(closest != null)
            {
                dockingManager.GetDockableEnemies().Remove(closest);
                dockingPosition = closest.GetDockingPosition();
                dockingDistance = minD;
                dockingStartTime = Time.time;
                dockingTime = dockingDistance / 3;
                dockingOngoing = true;
                dockingStart = transform.position;
                dockTarget = closest;
            }
        }

        if(dockingOngoing)
        { 
            playerShipRuntime.Invulnerable = true;
            Debug.DrawLine(dockingPosition, dockingPosition + Vector3.up);
            Debug.DrawLine(dockingPosition, dockingPosition + Vector3.down);
            Debug.DrawLine(dockingPosition, dockingPosition + Vector3.right);
            Debug.DrawLine(dockingPosition, dockingPosition + Vector3.left);
            shipCollider.enabled = false;
            float lerpCoef = (Time.time - dockingStartTime) / dockingTime;
            Vector3 newPos = Vector3.Lerp(dockingStart, dockingPosition, lerpCoef);
            invul_started = Time.time;
            transform.position = newPos;
            mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
            if (lerpCoef > 0.98f)
            {
                engineEffects.ForEach(x => x.enabled = false);
                shipAnimator.Play("ShipDock");
            }
        }
        else
        {
            if (playerShipRuntime.PlayerMaxY < playerShipRuntime.Position.y && vel.y > 0)
            {
                vel = new Vector2(vel.x, 0);
            }
            else if (playerShipRuntime.PlayerMinY > playerShipRuntime.Position.y && vel.y < 0)
            {
                vel = new Vector2(vel.x, 0);
            }

            if(shipCollider.enabled == false)
            {
                shipCollider.enabled = true;
            }

            engineEffects.ForEach(x => x.enabled = vel.x != 0 || vel.y != 0);
            body.velocity = vel;
            spriteRenderer.flipX = dir < 0;
            engineEffects.ForEach(x => x.flipX = dir < 0);
            engineEffects.ForEach(x => x.transform.localPosition = new Vector3(-1 * dir * Mathf.Abs(x.transform.localPosition.x), 0, 0));
            playerShipRuntime.Position = transform.position;
            mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }

        if (playerShipRuntime.HP > 10)
        {
            playerShipRuntime.HP = 10;
        }

        if (playerShipRuntime.Ammo > 45)
        {
            playerShipRuntime.HP = 45;
        }
    }

    private void fire()
    {
        if (lastFire + fireWait <= Time.time && playerShipRuntime.Ammo > 0)
        {
            PlayerSpaceAmmo ammo = Instantiate(ammoPrefab);
            ammo.transform.position = transform.position + new Vector3(0.5f * dir, 0, 0);
            ammo.SetDir(dir);
            lastFire = Time.time;
            playerShipRuntime.Ammo--;
        }
    }

    public void DoDamage(float damage)
    {
        if (!playerShipRuntime.Invulnerable)
        {
            flashCount = 0;
            playerShipRuntime.Invulnerable = true;
            playerShipRuntime.HP = playerShipRuntime.HP - damage;
        }
    }

    private void doInvuln()
    {
        if (playerShipRuntime.Invulnerable)
        {
            if (invul_started + invul_time <= Time.time)
            {
                playerShipRuntime.Invulnerable = false;
            }

            if (lastFlash + flashTime <= Time.time)
            {
                flash = !flash;
                lastFlash = Time.time;
            }

            if (flash)
            {
                if (flashCount < 2)
                {
                    hitSprite();
                    flashCount++;
                }
                else
                {
                    flashSprite();
                }
            }
            else
            {
                normalSprite();
            }
        }
        else
        {
            invul_started = Time.time;
        }


        if (!playerShipRuntime.Invulnerable && flash)
        {
            normalSprite();
            flash = false;
        }
    }

    private void hitSprite()
    {
        spriteRenderer.material.shader = shaderFlash;
        spriteRenderer.color = Color.red*0.9f;
    }

    private void flashSprite()
    {
        spriteRenderer.material.shader = shaderFlash;
        spriteRenderer.color = Color.white;
    }

    private void normalSprite()
    {
        spriteRenderer.material.shader = shaderSpritesDefault;
        spriteRenderer.color = Color.white;
    }

    public void AddGold()
    {
        playerShipRuntime.Gold += 243;
    }

    public void StopDocking()
    {
        dockingOngoing = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
        if (dockTarget != null)
        {
            dockTarget.StopDock();
        }
        shipAnimator.Play("ShipIdle");

    }

    public void AddHPandAmmo()
    {
        playerShipRuntime.HP += 0.67f;
        playerShipRuntime.Ammo += 6;
    }
}
