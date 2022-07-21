using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialTile : BoardPosition
{
    TileType previousType;
    public abstract void ActivateSpecial();
    public abstract void ActivateSuperSpecial();
}
