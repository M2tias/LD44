using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianShip : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D body;
    [SerializeField]
    private int dir = 1;
    [SerializeField]
    private PlayerSpaceAmmo ammoPrefab = null;
    [SerializeField]
    private float fireWait = 0.33f;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private GameObject boardSign;

    private float lastFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir = enemy.GetDir();
        transform.localScale = dir > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        boardSign.transform.localScale = dir > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
    }
}
