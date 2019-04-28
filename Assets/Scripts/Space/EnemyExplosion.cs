using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private float speedMax;
    [SerializeField]
    private float speedMin;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(speedMin, speedMax);
        direction = Vector3.Normalize(direction);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = transform.position;
        transform.position = v + direction * speed * Time.deltaTime;
    }
}
