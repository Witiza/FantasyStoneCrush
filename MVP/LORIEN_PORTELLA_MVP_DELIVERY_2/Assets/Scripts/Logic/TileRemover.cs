using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileRemover 
{
    
    public static void DestroyTile(BoardPosition tile)
    {
        switch (tile._type)
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
                Board.DestroyArea(1, tile.BoardPos);
                break;
            case TileType.HORIZONTAL_ROCKET:
                Board.DestroyRow((int)tile.BoardPos.x);
                break;
            case TileType.VERTICAL_ROCKET:
                Board.DestroyColumn((int)tile.BoardPos.y);
                break;
            default:
                break;
        }
        BoardEvents.NotifyDestroyed(tile.BoardPos,(int)tile._type); 
        tile._type = TileType.NULL;
    }
}
