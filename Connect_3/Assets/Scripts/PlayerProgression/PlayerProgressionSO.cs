using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class PlayerProgressionSO  : ScriptableObject
{
    public int CurrentLevel;
    public int MaxLevelUnlocked;
    public int Coins;

    public List<BoardConfig> levels;
    public Booster TileBooster;
    public Booster TurnBooster;
    public Booster ManaBooster;
    public IntEventBus CoinsChange;

    public BoardConfig GetCurrentLevelBoard()
    {
        return levels[CurrentLevel];
    }
    public void ResetProgression()
    {
        CurrentLevel = 0;
        Coins = 0;
        TileBooster.amount = 0;
        TurnBooster.amount = 0;
        ManaBooster.amount = 0;
    }

    public void AttemptToBuyBooster(Booster booster)
    {
        if(booster.cost <= Coins)
        {
            booster.amount = 10;
            Coins -= booster.cost;
            CoinsChange.NotifyEvent(booster.cost);
        }
    }

    public void AddCoins(int coins)
    {
        CoinsChange.NotifyEvent(coins);
        Coins += coins;
    }
}