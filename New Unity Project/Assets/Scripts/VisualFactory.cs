using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualFactory : MonoBehaviour
{
    public static List<Tile> tiles = new List<Tile>();
    public GameObject tile_prefab;
    public GameObject test;
    public  void Awake()  
    {
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
        Tile tile = GetTileAtPos(pos1);
        Tile other = GetTileAtPos(pos2);
        tile.SetBoardPosition(pos2);
        other.SetBoardPosition(pos1);
    }

    private void BoardEvents_TileDestroyed(Vector2 obj)
    {
        Tile tile = GetTileAtPos(obj);
        tiles.Remove(tile);
        Destroy(tile.gameObject);
    }

    private void BoardEvents_TileMoved(Vector2 origin, Vector2 destination)
    {
        Tile tile = GetTileAtPos(origin);
        tile.SetBoardPosition(destination);
    }

    private void BoardEvents_TileCreated(Vector2 pos, int type)
    {
        Tile tile = Object.Instantiate(tile_prefab).GetComponent<Tile>();
        //Initialization should be methods in a viewtilehandler or sumthing
        tile.InitializeTile((TileType)type, pos);
        tiles.Add(tile);
    }

    public static Tile GetTileAtPos(Vector2 pos)
    {
        Tile tile = tiles.Find(e => e.board_pos == pos);
        if(tile == null)
        {
            Debug.Log($"COULDNT FIND TILE AT {pos.x},{pos.y}");
        }
        return tile;
    }
}
