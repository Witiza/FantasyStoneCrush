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

    public void AttemptToBuy()
    {
        if (model.canBuy(progression))
        {
            switch (model.type)
            {
                case CostType.COINS:
                    progression.ModifyCoins(-model.cost);
                    break;
                case CostType.GEMS:
                    progression.ModifyGems(-model.cost);
                    break;
            }
            boughtEvent.NotifyEvent(model.id);
        }
    }
}
