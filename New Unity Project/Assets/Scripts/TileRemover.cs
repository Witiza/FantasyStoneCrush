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
                return;
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
                Board.DestroyArea(1, tile.board_position);
                break;
            case TileType.HORIZONTAL_ROCKET:
                Board.DestroyRow((int)tile.board_position.x);
                break;
            case TileType.VERTICAL_ROCKET:
                Board.DestroyColumn((int)tile.board_position.y);
                break;
            default:
                break;
        }
        tile.type = TileType.NULL;
        BoardEvents.NotifyDestroyed(tile.board_position); 
    }
}
