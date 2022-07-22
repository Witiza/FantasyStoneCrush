using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTile : SpecialTile
{
    bool horizontal = false;
    public override void DestroyTile()
    {
        ActivateSpecial();
    }
    public override void ActivateSpecial()
    {
        
    }
    public override void ActivateSuperSpecial()
    {
        throw new System.NotImplementedException();
    }
}
