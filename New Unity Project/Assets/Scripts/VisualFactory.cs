using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualFactory : MonoBehaviour
{
    List<Tile> tiles = new List<Tile>();
    public GameObject tile_prefab;

    public  void Awake()  
    {
        BoardEvents.TileCreated += BoardEvents_TileCreated;
        BoardEvents.TileMoved += BoardEvents_TileMoved;
        BoardEvents.TileDestroyed += BoardEvents_TileDestroyed;
        BoardEvents.TileSelected += BoardEvents_TileSelected;
        BoardEvents.TileUnselected += BoardEvents_TileUnselected;
    }

    private void BoardEvents_TileUnselected(Vector2 obj)
    {
        throw new System.NotImplementedException();
    }

    private void BoardEvents_TileSelected(Vector2 obj)
    {
        throw new System.NotImplementedException();
    }

    private void BoardEvents_TileDestroyed(Vector2 obj)
    {
        throw new System.NotImplementedException();
    }

    private void BoardEvents_TileMoved(Vector2 arg1, Vector2 arg2)
    {
        throw new System.NotImplementedException();
    }

    private void BoardEvents_TileCreated(Vector2 pos, int type)
    {
        Tile tile = Object.Instantiate(tile_prefab).GetComponent<Tile>();
        tile.InitializeTile((TileType)type, pos);
    }


}
