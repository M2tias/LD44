﻿using System.Collections;
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
    private float hp;
    public float HP { get { return hp; } set { hp = value; } }

    [SerializeField]
    private int ammo;
    public int Ammo { get { return ammo; } set { ammo = value; } }

    public float PlayerMaxY { get { return 7.5f; } }
    public float PlayerMinY { get { return -5f; } }
}
