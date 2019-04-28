using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyRuntime", menuName = "New EnemyRuntime")]
public class EnemyRuntime : ScriptableObject
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position { get { return position; } set { position = value; } }

    [SerializeField]
    private int hp;
    public int HP { get { return hp; } set { hp = value; } }
}
