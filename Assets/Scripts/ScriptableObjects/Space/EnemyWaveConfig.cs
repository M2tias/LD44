using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWaveConfig", menuName = "New EnemyWaveConfig")]
public class EnemyWaveConfig : ScriptableObject
{
    [SerializeField]
    private List<Wave> waves;
    public List<Wave> Waves { get { return waves; } }
}

[Serializable]
public class Wave
{
    // Time in seconds, starts "running" when the previous wave was spawned
    public float prepTime;
    public List<WaveEnemy> enemies;
}

[Serializable]
public class WaveEnemy
{
    public Vector3 position; // relative to player
    public EnemyType type;
}
