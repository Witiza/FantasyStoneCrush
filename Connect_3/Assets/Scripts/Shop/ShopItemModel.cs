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
    public int cost { get; private set; }

    public string name;
    public int id;
    public CostType type;
    GameConfigService _gameConfigService = null;


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

    public void SetPrice()
    {
        if(_gameConfigService == null)
            _gameConfigService = ServiceLocator.GetService<GameConfigService>();
        cost = _gameConfigService.GetShopCost(id);
    }
}

