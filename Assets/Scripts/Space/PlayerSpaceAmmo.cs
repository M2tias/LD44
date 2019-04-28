using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// all ammos in space, not just player
public class PlayerSpaceAmmo : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float speed = 300f;
    [SerializeField]
    private float damage = 1;
    [SerializeField]
    private bool playerAmmo = false;

    private int dir = 1;
    private float lifeTime = 5f;
    private float created = 0f;

    // Start is called before the first frame update
    void Start()
    {
        created = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = new Vector2(speed * Time.deltaTime * dir, 0);
        spriteRenderer.flipX = dir < 0;
        if (created + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }

    public void SetDir(int d)
    {
        dir = d;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !playerAmmo)
        {
            PlayerShip ship = collision.gameObject.GetComponent<PlayerShip>();
            if (ship == null) return;
            ship.DoDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Enemy" && playerAmmo)
        {
            Enemy ship = collision.gameObject.GetComponent<Enemy>();
            if (ship == null) return;
            ship.DoDamage(damage);
            Destroy(gameObject);
        }
    }
}
