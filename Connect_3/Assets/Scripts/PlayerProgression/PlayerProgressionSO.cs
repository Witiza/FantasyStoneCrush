using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CreateAssetMenu]
public class PlayerProgressionSO  : ScriptableObject
{
    public int CurrentLevel;
    public int MaxLevelUnlocked;
    public int Coins;
    public int Gems;

    public List<BoardConfig> levels;
    public Booster TileBooster;
    public Booster TurnBooster;
    public Booster ManaBooster;
    public IntEventBus CoinsChange;
    public IntEventBus GemsChange;

    public BoardConfig GetCurrentLevelBoard()
    {
        return levels[CurrentLevel];
    }
    public void ResetProgression()
    {
        CurrentLevel = 0;
        MaxLevelUnlocked = 0;
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
        Coins += coins;
        CoinsChange.NotifyEvent(coins);
    }

    public void AddGems(int gems)
    {
        Gems += gems;
        GemsChange.NotifyEvent(gems);
    }


    public void SaveGame()
    {
        SaveSystem.SaveGame(new ProgressionSerializable(CurrentLevel,MaxLevelUnlocked,Coins,Gems));
    }
    public void LoadGame()
    {
        ProgressionSerializable tmp = SaveSystem.LoadGame();
        if (tmp!=null)
        {
            CurrentLevel = tmp.CurrentLevel;
            MaxLevelUnlocked = tmp.MaxLevelUnlocked;
            Coins = tmp.Coins;
        }
    }
}
