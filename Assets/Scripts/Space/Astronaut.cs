using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astronaut : MonoBehaviour
{
    private Vector3 dir;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedDecay;
    private float rotate = 1;
    private float rotateDecay = 0.2f;

    private float lifeTime = 60f;
    private float created = 0f;
    private float maxY { get { return 7.5f; } }
    private float minY { get { return -5f; } }

    // Start is called before the first frame update
    void Start()
    {
        created = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = transform.position;
        transform.position = v + dir * speed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotate);
        speed = speed - speedDecay * Time.deltaTime;
        rotate = rotate - rotateDecay * Time.deltaTime;
        if (speed < 0) speed = 0;
        if (rotate < 0.1f) rotate = 0.1f;

        if (created + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }

        v = transform.position;

        if (v.y > maxY)
        {
            transform.position = new Vector3(v.x, maxY, v.z);
        }
        else if (v.y < minY)
        {
            transform.position = new Vector3(v.x, minY, v.z);
        }
    }

    public void SetDirection(Vector3 d)
    {
        dir = d;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerShip ship = collision.gameObject.GetComponent<PlayerShip>();
            if (ship != null)
            {
                ship.AddGold();
                Destroy(gameObject);
            }
        }
    }
}
