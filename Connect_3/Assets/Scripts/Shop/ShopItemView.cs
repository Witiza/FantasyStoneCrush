using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItemView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text cost;
    [SerializeField]
    private TMP_Text reward;

    [SerializeField]
    ShopItemModel model;

    [SerializeField]
    PlayerProgressionService progression;

    [SerializeField]
    IntEventBus boughtEvent;

    GameIAPService IAPService;
    GameAdsService ADService;

    private void Awake()
    {
        IAPService = ServiceLocator.GetService<GameIAPService>();
        ADService = ServiceLocator.GetService<GameAdsService>();
    }
    public void UpdateView()
    {
        model.SetPrice();
        cost.text = model.cost.ToString();
        switch (model.type)
        {
            case CostType.COINS:
                cost.text += " C.";
                break;
            case CostType.GEMS:
                cost.text += " G.";
                break;
            case CostType.AD:
                cost.text = ">";
                break;
            case CostType.MONEY:
                UpdateIAPProduct();
                break;
        }

        if (model.canBuy(progression))
        {
            cost.color = Color.black;
        }
        else
        {
            cost.color = Color.red;
        }
        reward.text = model.name;
    }

    public void UpdateIAPProduct()
    {
        if(IAPService.IsReady)
        {
            cost.text = IAPService.GetLocalizedPrice(model.name);
        }
        else
        {
            cost.text = "Unavailable";
        }
    }

    public void UpdateADProduct()
    {
        if (ADService.IsAdReady)
        {
            cost.text = ">";
        }
        else
        {
            cost.text = "Unavailable";
        }
    }


    public async void AttemptToBuy()
    {
        if (model.canBuy(progression))
        {
            switch (model.type)
            {
                case CostType.COINS:
                    progression.ModifyCoins(-model.cost);
                    boughtEvent.NotifyEvent(model.id);
                    break;
                case CostType.GEMS:
                    progression.ModifyGems(-model.cost);
                    boughtEvent.NotifyEvent(model.id);
                    break;
                case CostType.AD:
                    if(ADService.Initialized && await ADService.ShowAd())
                    {
                        boughtEvent.NotifyEvent(model.id);
                    }
                    break;
                case CostType.MONEY:
                    if(IAPService.Initialized && await IAPService.StartPurchase(model.name))
                    {
                        boughtEvent.NotifyEvent(model.id);
                    }
                    break;
            }

        }
    }
}
