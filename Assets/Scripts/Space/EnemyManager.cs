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
    [SerializeField]
    private EnemyWaveConfig enemyWaveConfig;
    private float lastWaveStarted = 0f;
    private int nextWaveId = 0;
    [SerializeField]
    private GameObject waveWarning;
    private bool NoMoreWaves = false;
    [SerializeField]
    private PlayerShipRuntime playerShipRuntime;
    [SerializeField]
    private UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<Enemy>();
        lastWaveStarted = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (NoMoreWaves)
        {
            if (enemies.Count == 0 && NoMoreWaves)
            {
                uiManager.PartialWin();
            }

            Debug.Log("Win soon!");
            return;
        }

        Wave nextWave = enemyWaveConfig.Waves[nextWaveId];
        float nextPrepTime = nextWave.prepTime;
        if(Time.time >= nextPrepTime + lastWaveStarted)
        {
            Debug.Log("New wave!");
            foreach(WaveEnemy we in nextWave.enemies)
            {
                Enemy enemy = instantiateEnemy(we.type);
                enemy.SetDockingManager(dockingManager);
                Vector2 pv = playerShipRuntime.Position;
                enemy.transform.position = new Vector3(pv.x, pv.y, 0) + we.position;
                enemies.Add(enemy);
            }
            lastWaveStarted = Time.time;
            nextWaveId++;
            NoMoreWaves = nextWaveId >= enemyWaveConfig.Waves.Count;
            StartCoroutine(FlashWaveWarning());
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

    IEnumerator FlashWaveWarning()
    {
        waveWarning.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(false);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(false);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(false);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(true);
        yield return new WaitForSeconds(0.33f);
        waveWarning.SetActive(false);
        yield return new WaitForSeconds(0.33f);
    }
}
