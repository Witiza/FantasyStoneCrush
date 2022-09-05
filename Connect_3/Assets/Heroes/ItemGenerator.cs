using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemGenerator 
{
    public Vector2Int normalStatAmount;
    public Vector2Int premiumStatAmount;

    public Multipliers BaseMultipliers;
    public Multipliers NormalVariance;
    public Multipliers PremiumVariance;

    public PlayerInventory inventory;

    public ItemModel GenerateItem(bool premium)
    {
        Multipliers result = new Multipliers();
        Multipliers multi = premium ? PremiumVariance : NormalVariance;
        Vector2Int variableAmount = premium ? premiumStatAmount : normalStatAmount;

        int amount = Random.Range(variableAmount.x, variableAmount.y+1);
        for(int i = 0; i < amount; i++)
        {
            int stat = Random.Range(0, 4);
            switch (stat)
            {
                case 0:
                    result.CritChance += BaseMultipliers.CritChance*Random.Range(0, multi.CritChance);
                    break;
                case 1:
                    result.NormalMultiplier += BaseMultipliers.NormalMultiplier * Random.Range(0, multi.NormalMultiplier);
                    break;
                case 2:
                    result.CriticalMultiplier += BaseMultipliers.CriticalMultiplier * Random.Range(0, multi.CriticalMultiplier);
                    break;
                case 3:
                    result.ManaGainMultiplier += BaseMultipliers.ManaGainMultiplier * Random.Range(0, multi.ManaGainMultiplier);
                    break;
            }
        }

        ItemModel item = new ItemModel();
        item.ItemMultipliers = result;
        item.name = "Sample Item No" + Random.Range(0, 100).ToString();
        inventory.AddItem(item);
        return item;
    }
}
