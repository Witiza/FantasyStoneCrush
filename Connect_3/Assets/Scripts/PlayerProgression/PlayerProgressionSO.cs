using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class PlayerProgressionSO  : ScriptableObject
{
    public int CurrentLevel;
    public int MaxLevelUnlocked;
    public int Coins;

    public List<BoardConfig> levels;
    public int TurnBoosterAmount;
    public int ManaBoosterAmount;
    public int TileBoosterAmount;

    public BoardConfig GetCurrentLevelBoard()
    {
        return levels[CurrentLevel];
    }
    public void ResetProgression()
    {
        CurrentLevel = 0;
        Coins = 0;
        TurnBoosterAmount = 0;
        ManaBoosterAmount = 0;
        TileBoosterAmount = 0;
    }
}