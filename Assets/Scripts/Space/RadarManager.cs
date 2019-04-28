using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private PlayerShipRuntime playerRuntime;
    float playerRadarMaxY = 1;
    float playerRadarMinY = -25;
    public float MaxY { get { return 7.5f; } }
    public float MinY { get { return -5f; } }

    float spaceWidth = 80;
    float radarWidth = 170;

    [SerializeField]
    private GameObject playerRadarIcon;
    [SerializeField]
    private GameObject civilianBigRadarIconPrefab;
    [SerializeField]
    private GameObject fighterBigRadarIconPrefab;
    [SerializeField]
    private GameObject cargoBigRadarIconPrefab;
    [SerializeField]
    private GameObject militaryBigRadarIconPrefab;
    [SerializeField]
    private GameObject civilianSmallRadarIconPrefab;
    [SerializeField]
    private GameObject fighterSmallRadarIconPrefab;
    [SerializeField]
    private GameObject cargoSmallRadarIconPrefab;
    [SerializeField]
    private GameObject militarySmallRadarIconPrefab;

    private Dictionary<Enemy, GameObject> instantiatedBigIcons;
    private Dictionary<Enemy, GameObject> instantiatedSmallIcons;

    // Start is called before the first frame update
    void Start()
    {
        instantiatedBigIcons = new Dictionary<Enemy, GameObject>();
        instantiatedSmallIcons = new Dictionary<Enemy, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        drawPlayerMarker();
        List<Enemy> enemies = enemyManager.Enemies;

        for(int i = enemies.Count-1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                continue;
            }
        }

        foreach (Enemy enemy in enemies)
        {

            if (!instantiatedBigIcons.ContainsKey(enemy))
            {

                GameObject prefab = getBigPrefab(enemy.GetEnemyType());

                if (prefab != null)
                {
                    instantiatedBigIcons.Add(enemy, Instantiate(prefab, transform));
                }
            }

            if (!instantiatedSmallIcons.ContainsKey(enemy))
            {

                GameObject prefab = getSmallPrefab(enemy.GetEnemyType());

                if (prefab != null)
                {
                    instantiatedSmallIcons.Add(enemy, Instantiate(prefab, transform));
                }
            }

            GameObject iconBig = instantiatedBigIcons[enemy];
            GameObject iconSmall = instantiatedSmallIcons[enemy];
            float enemyXDistance = Mathf.Abs(enemy.transform.position.x - playerRuntime.Position.x);
            float enemyPercentage = enemyXDistance / (spaceWidth / 2);
            enemyPercentage = enemyPercentage > 1 ? 1 : enemyPercentage;
            float enemyPosSign = enemy.transform.position.x > playerRuntime.Position.x ? 1 : -1;
            float enemyRadarPosX = enemyPosSign * (radarWidth / 2) * enemyPercentage;
            float enemyRadarPosY = getRadarYPos(enemy.transform.position.y, enemyRadarPosX, true);
            iconBig.transform.localPosition = new Vector3(enemyRadarPosX, enemyRadarPosY, 0);
            iconSmall.transform.localPosition = new Vector3(enemyRadarPosX, enemyRadarPosY, 0);

            if (enemyPercentage > 0.6)
            {
                iconBig.SetActive(false);
                iconSmall.SetActive(true);
            }
            else
            {
                iconBig.SetActive(true);
                iconSmall.SetActive(false);
            }
        }

        List<GameObject> deleted = instantiatedBigIcons.Where(x => x.Key == null).Select(x => x.Value).ToList();
        for (int i = deleted.Count - 1; i >= 0; i--)
        {
            Destroy(deleted[i]);
        }
    }

    private GameObject getBigPrefab(EnemyType type)
    {
        if (type == EnemyType.Cargo)
        {
            return cargoBigRadarIconPrefab;
        }
        else if (type == EnemyType.Civilian)
        {
            return civilianBigRadarIconPrefab;
        }
        else if (type == EnemyType.Fighter)
        {
            return fighterBigRadarIconPrefab;
        }
        else if (type == EnemyType.Military)
        {
            return militaryBigRadarIconPrefab;
        }

        return null;
    }

    private GameObject getSmallPrefab(EnemyType type)
    {
        if (type == EnemyType.Cargo)
        {
            return cargoSmallRadarIconPrefab;
        }
        else if (type == EnemyType.Civilian)
        {
            return civilianSmallRadarIconPrefab;
        }
        else if (type == EnemyType.Fighter)
        {
            return fighterSmallRadarIconPrefab;
        }
        else if (type == EnemyType.Military)
        {
            return militarySmallRadarIconPrefab;
        }

        return null;
    }

    private void drawPlayerMarker()
    {
        /*float playerMaxY = playerRuntime.PlayerMaxY;
        float playerMinY = playerRuntime.PlayerMinY;
        float playerRange = Mathf.Abs(playerMaxY - playerMinY);
        float playerRadarRange = Mathf.Abs(playerRadarMaxY - playerRadarMinY);
        float playerHeight = Mathf.Abs(playerRuntime.Position.y - playerMinY);
        float playerPercentage = playerHeight / playerRange;*/
        float playerRadarHeight = getRadarYPos(playerRuntime.Position.y);//playerPercentage * playerRadarRange;
        Vector3 p = playerRadarIcon.transform.localPosition;
        playerRadarIcon.transform.localPosition = new Vector3(p.x, playerRadarHeight + 4, p.z);

        //Debug.Log("playerRange: " + playerRange + ", PlayerRadarRange: " + playerRadarRange);
        //Debug.Log("%: " + playerPercentage + ", current height: " + playerRadarHeight);
    }

    private float getRadarYPos(float yPos, float xPos = 0, bool accountForX = false)
    {
        float entityRange = Mathf.Abs(MaxY - MinY);
        float radarRange = Mathf.Abs(playerRadarMaxY - playerRadarMinY);
        float entityHeight = Mathf.Abs(yPos - MinY);
        float entityPercentage = entityHeight / entityRange;
        float entityRadarHeight = entityPercentage * radarRange;

        if (accountForX)
        {
            float newHeight = entityRadarHeight - Mathf.Sqrt(1200 - 0.15f * xPos * xPos) * 0.23f + 14;
            if (float.IsNaN(newHeight))
            {
                Debug.Log(xPos + " ---- " + (170 - 0.14f * xPos * xPos));
                entityRadarHeight = 0;
            }
            else
            {
                entityRadarHeight = newHeight;
            }
        }

        return entityRadarHeight;
    }
}
