using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
//using TiledSharp;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    [SerializeField]
    private LevelConfig levelConfig = null;
    [SerializeField]
    private Texture2D spriteAtlas = null;
    private Sprite[] sprites = null;
    /*[SerializeField]
    private Tile tilePrefab = null;*/
    private bool levelLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Tilesets/" + spriteAtlas.name);

    }

    // Update is called once per frame
    void Update()
    {
        if(!levelLoaded)
        {
            LoadLevel(levelConfig.Levels[0].text, levelConfig.Tileset.text);
        }
    }

    public void LoadLevel(string level, string tilesetRef)
    {
        /*XDocument doc = XDocument.Parse(level);
        XDocument tilesetDoc = XDocument.Parse(tilesetRef);
        TmxMap map = new TmxMap(doc);
        map.AddTilesetsAsXContainers(tilesetDoc.Elements("tileset").ToList());
        TmxTileset tileset = map.Tilesets[0];


        int layerInt = 0;
        foreach (TmxLayer layer in map.Layers)
        {
            foreach (TmxLayerTile tile in layer.Tiles)
            {
                if (layer.Name != "colliders") continue;

                if (tile.Gid <= 0 || !tileset.Tiles.ContainsKey(tile.Gid)) continue;
                TmxTilesetTile tilesetTile = tileset.Tiles[tile.Gid - 1];
                Tile tilePrefab = levelConfig.Colliders.Select(x => x.gameObject.GetComponent<Tile>()).Where(x => x.ID == tilesetTile.Id).FirstOrDefault();
                if (tilePrefab == null)
                {
                    Debug.Log("Sad :(");
                    continue;
                }

                Tile tileObj = Instantiate(tilePrefab);
                tileObj.transform.position = new Vector3(tile.X, 5 - tile.Y, 0);

            }
        }
        levelLoaded = true;*/
    }

}
