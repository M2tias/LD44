using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private Enemy militaryEnemyPrefab;
    [SerializeField]
    private Enemy civilianEnemyPrefab;
    [SerializeField]
    private Enemy cargoEnemyPrefab;
    [SerializeField]
    private Enemy fighterEnemyPrefab;
    private List<Enemy> enemies;
    public List<Enemy> Enemies { get { return enemies; } }
    [SerializeField]
    private DockingManager dockingManager;
    private float shipCount = 1;


    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shipCount > 0)
        {
            Enemy militaryEnemy = instantiateEnemy(EnemyType.Military);
            militaryEnemy.SetDockingManager(dockingManager);
            Vector3 v = militaryEnemy.transform.position;
            militaryEnemy.transform.position = new Vector3(v.x, -5, v.z);
            shipCount--;
        }
    }

    public Enemy instantiateEnemy(EnemyType type)
    {
        Enemy enemy = null;
        if (type == EnemyType.Cargo)
        {
            enemy = Instantiate(cargoEnemyPrefab);
        }
        else if (type == EnemyType.Military)
        {
            enemy = Instantiate(militaryEnemyPrefab);
        }
        else if (type == EnemyType.Fighter)
        {
            enemy = Instantiate(fighterEnemyPrefab);
        }
        else if (type == EnemyType.Civilian)
        {
            enemy = Instantiate(civilianEnemyPrefab);
        }

        enemies.Add(enemy);
        return enemy;
    }
}
