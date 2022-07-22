using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTile : SpecialTile
{
    public override void DestroyTile()
    {
        ActivateSpecial();
        type = TileType.NULL;
    }
    public override void ActivateSpecial()
    {
        GameObject.FindGameObjectWithTag("Board").GetComponent<Board>().DestroyArea(1,board_position);
    }
    public override void ActivateSuperSpecial()
    {
        throw new System.NotImplementedException();
    }
}
