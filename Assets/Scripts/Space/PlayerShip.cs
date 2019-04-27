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
        playerShipRuntime.HP = 5;
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

        if (playerShipRuntime.HP > 5)
        {
            playerShipRuntime.HP = 5;
        }

        body.velocity = vel;
        spriteRenderer.flipX = dir < 0;
        engineEffects.ForEach(x => x.flipX = dir < 0);
        engineEffects.ForEach(x => x.transform.localPosition = new Vector3(-1 * dir * Mathf.Abs(x.transform.localPosition.x), 0, 0));
        playerShipRuntime.Position = transform.position;
        mainCamera.transform.position = new Vector3(transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
    }

    private void fire()
    {
        if (lastFire + fireWait <= Time.time)
        {
            PlayerSpaceAmmo ammo = Instantiate(ammoPrefab);
            ammo.transform.position = transform.position + new Vector3(0.5f * dir, 0, 0);
            ammo.SetDir(dir);
            lastFire = Time.time;
        }
    }

    public void DoDamage(int damage)
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
}
