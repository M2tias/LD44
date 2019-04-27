using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerShipRuntime", menuName = "New PlayerShipRuntime")]
public class PlayerShipRuntime : ScriptableObject
{
    [SerializeField]
    private Vector2 position;
    public Vector2 Position { get { return position; } set { position = value; } }

    [SerializeField]
    private bool invulnerable;
    public bool Invulnerable { get { return invulnerable; } set { invulnerable = value; } }

    [SerializeField]
    private int gold;
    public int Gold { get { return gold; } set { gold = value; } }

    [SerializeField]
    private int hp;
    public int HP { get { return hp; } set { hp = value; } }
}
