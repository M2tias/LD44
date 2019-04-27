using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "New LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [SerializeField]
    private List<TextAsset> levels = null;
    public List<TextAsset> Levels { get { return levels; } }

    [SerializeField]
    private TextAsset tileset = null;
    public TextAsset Tileset { get { return tileset; } }

    [SerializeField]
    private List<PolygonCollider2D> colliders;
    public List<PolygonCollider2D> Colliders { get { return colliders; } }

}
