using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostType
{
    NULL,
    COINS,
    GEMS
}
[System.Serializable]
public  class ShopItemModel 
{
    public string name;
    public int cost;
    public int id;
    public CostType type;

    public bool canBuy(PlayerProgressionService player)
    {
        bool ret = false;
        switch (type)
        {
            case CostType.COINS:
                if (player.Coins >= cost)
                {
                    ret = true;
                }
                break;
            case CostType.GEMS:
                if (player.Gems >= cost)
                {
                    ret = true;
                }
                break;
        }
        return ret;
    }
}

