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

    private void Awake()
    {
        IAPService = ServiceLocator.GetService<GameIAPService>();
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
            cost.text = IAPService.GetLocalizedPrice("Test1");
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
                    if(await ServiceLocator.GetService<GameAdsService>().ShowAd())
                    {
                        boughtEvent.NotifyEvent(model.id);
                    }
                    break;
                case CostType.MONEY:
                    if(await IAPService.StartPurchase("Test1"))
                    {
                        boughtEvent.NotifyEvent(model.id);
                    }
                    break;
            }

        }
    }
}
