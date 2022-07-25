using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualHandler : MonoBehaviour
{
    public static List<VisualTile> tiles = new List<VisualTile>();
    public GameObject tile_prefab;
    public  void Awake()  
    {
        tiles.Clear();
        BoardEvents.TileCreated += BoardEvents_TileCreated;
        BoardEvents.TileMoved += BoardEvents_TileMoved;
        BoardEvents.TileSwapped += BoardEvents_TileSwapped;
        BoardEvents.TileDestroyed += BoardEvents_TileDestroyed;
    }

    public void OnDestroy()
    {
        BoardEvents.TileCreated -= BoardEvents_TileCreated;
        BoardEvents.TileMoved -= BoardEvents_TileMoved;
        BoardEvents.TileSwapped -= BoardEvents_TileSwapped;
        BoardEvents.TileDestroyed -= BoardEvents_TileDestroyed;
    }

    private void BoardEvents_TileSwapped(Vector2 pos1, Vector2 pos2)
    {
        VisualTile tile = GetTileAtPos(pos1);
        VisualTile other = GetTileAtPos(pos2);
        tile.SetBoardPosition(pos2);
        other.SetBoardPosition(pos1);
    }

    private void BoardEvents_TileDestroyed(Vector2 obj,int type)
    {
        VisualTile tile = GetTileAtPos(obj);
        tiles.Remove(tile);
        Destroy(tile.gameObject);
    }

    private void BoardEvents_TileMoved(Vector2 origin, Vector2 destination)
    {
        VisualTile tile = GetTileAtPos(origin);
        tile.SetBoardPosition(destination);
    }

    private void BoardEvents_TileCreated(Vector2 pos, int type)
    {
        VisualTile tile = Object.Instantiate(tile_prefab).GetComponent<VisualTile>();
        //Initialization should be methods in a viewtilehandler or sumthing
        tile.InitializeTile((TileType)type, pos);
        tiles.Add(tile);
    }

    public static VisualTile GetTileAtPos(Vector2 pos)
    {
        VisualTile tile = tiles.Find(e => e.BoardPos == pos);
        if(tile == null)
        {
            Debug.Log($"COULDNT FIND TILE AT {pos.x},{pos.y}");
        }
        return tile;
    }
}
