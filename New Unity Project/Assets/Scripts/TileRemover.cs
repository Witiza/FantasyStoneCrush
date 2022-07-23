using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileRemover 
{
    public static void DestroyTile(BoardPosition tile)
    {
        switch (tile.type)
        {
            case TileType.NULL:
                break;
            case TileType.SHIELD:
                break;
            case TileType.DAGGER:
                break;
            case TileType.ARROW:
                break;
            case TileType.WAND:
                break;
            case TileType.CHALICE:
                break;
            case TileType.BOMB:
                break;
            case TileType.ROCKET:
                break;
            default:
                break;
        }
        tile.type = TileType.NULL;
        BoardEvents.NotifyDestroyed(tile.board_position); 
    }
}
