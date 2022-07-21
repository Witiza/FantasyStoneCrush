using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : BoardPosition
{
    public override void DestroyTile()
    {
        Object.Destroy(target_tile.gameObject);
        target_tile = null;
    }

}
