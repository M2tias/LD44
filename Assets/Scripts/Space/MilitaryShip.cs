using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryShip : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private int dir = 1;
    [SerializeField]
    private SpriteRenderer front;
    [SerializeField]
    private SpriteRenderer back;
    [SerializeField]
    private PlayerSpaceAmmo ammoPrefab = null;
    [SerializeField]
    private float fireWait = 0.33f;
    [SerializeField]
    private Enemy enemy;

    private float lastFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir = enemy.GetDir();
        front.flipX = dir < 1;
        back.flipX = dir < 1;

        if(dir > 0)
        {
            front.transform.localPosition = new Vector3(1, 0, 0);
            back.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            front.transform.localPosition = new Vector3(0, 0, 0);
            back.transform.localPosition = new Vector3(1, 0, 0);
        }

        if (enemy.GetShooting() && lastFire + fireWait <= Time.time)
        {
            PlayerSpaceAmmo ammo = Instantiate(ammoPrefab);
            float x = dir > 0 ? 1.4065f : -0.4065f;
            ammo.transform.position = transform.position + new Vector3(x, -0.2189f, 0);
            ammo.SetDir(dir);
            lastFire = Time.time;
        }
    }
}
