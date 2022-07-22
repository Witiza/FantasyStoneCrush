using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : BoardPosition
{
    public override void DestroyTile()
    {
        type = TileType.NULL;
    }

    public override bool IsBaseTile()
    {
        return true;
    }

    public override bool IsSpecialTile()
    {
        return false;
    }

}
