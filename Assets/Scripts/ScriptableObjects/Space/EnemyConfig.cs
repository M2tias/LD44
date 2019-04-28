using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "New EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField]
    private EnemyType enemyType;
    public EnemyType EnemyType { get { return enemyType; } set { enemyType = value; } }

}
