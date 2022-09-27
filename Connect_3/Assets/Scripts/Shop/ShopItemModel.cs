using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CostType
{
    NULL,
    COINS,
    GEMS,
    AD,
    MONEY
}
[System.Serializable]
public  class ShopItemModel 
{
    public int cost { get; private set; }

    public string name;
    public int id;
    public CostType type;
    GameConfigService _gameConfigService = null;
    GameAdsService _gameAdsService = null;
    GameIAPService _gameIAPService = null;

    public bool canBuy(PlayerProgressionService player)
    {
        bool ret = true;
        switch (type)
        {
            case CostType.COINS:
                if (player.Coins < cost)
                {
                    ret = true;
                }
                break;
            case CostType.GEMS:
                if (player.Gems < cost)
                {
                    ret = true;
                }
                break;
            case CostType.AD:
                if(_gameAdsService.IsAdReady)
                {
                    ret = true;
                }
                break;
            case CostType.MONEY:
                if(_gameIAPService.IsReady)
                {
                    ret =true;
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

        if(_gameAdsService == null)
            _gameAdsService = ServiceLocator.GetService<GameAdsService>();

        if (_gameIAPService == null)
            _gameIAPService = ServiceLocator.GetService<GameIAPService>();
    }
}

