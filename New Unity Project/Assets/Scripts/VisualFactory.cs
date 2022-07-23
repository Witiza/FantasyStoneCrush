using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualFactory : MonoBehaviour
{
    public static List<Tile> tiles = new List<Tile>();
    public GameObject tile_prefab;

    public  void Awake()  
    {
        BoardEvents.TileCreated += BoardEvents_TileCreated;
        BoardEvents.TileMoved += BoardEvents_TileMoved;
        BoardEvents.TileDestroyed += BoardEvents_TileDestroyed;

    }

    public void OnDestroy()
    {
        BoardEvents.TileCreated -= BoardEvents_TileCreated;
        BoardEvents.TileMoved -= BoardEvents_TileMoved;
        BoardEvents.TileDestroyed -= BoardEvents_TileDestroyed;
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