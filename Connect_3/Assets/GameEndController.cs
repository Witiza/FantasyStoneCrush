using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndController : MonoBehaviour
{
    public PlayerProgressionSO PlayerProgression;
   
    public float ScoreMultiplier;
    public float MovesMultiplier;
    [Header("amount/(LowerLevelMultiplier*(maxlvl-lvl))")]
    public float LowerLevelMultiplier;
    public GameEndEventBus GameWon;
    public GameEndEventBus GameLost;
    public StringEventBus LevelEvent;
    public IntEventBus CoinsWon;

    void EndGame(GameEndInfo info)
    {
        gameObject.SetActive(true);
        CalculateEndCoins(info._remainingMoves, info._score,info._level,info._highestLevel);
        if(info._gameWon)
        {
            if(PlayerProgression.CurrentLevel<PlayerProgression.levels.Count)
            {
                PlayerProgression.CurrentLevel++;
            }
        }
    }

    void CalculateEndCoins(int moves,int score, int level, int maxLevel)
    {
        int amount = Mathf.RoundToInt(moves * MovesMultiplier + score * ScoreMultiplier);
        if (level == maxLevel)
        {
            CoinsWon.NotifyEvent(amount);
        }
        else
        {
            amount = Mathf.RoundToInt(amount / (LowerLevelMultiplier * (maxLevel - level)));
            amount = amount>=1?amount:1;
            CoinsWon.NotifyEvent(amount);
        }
        PlayerProgression.Coins += amount;
    }

    public void PlayNextGameLevel()
    {
        string name = "Level "+PlayerProgression.CurrentLevel;
        LevelEvent.NotifyEvent(name);
    }

    private void Awake()
    {
        GameWon.Event += EndGame;
        GameLost.Event += EndGame;
        gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        GameWon.Event -= EndGame;
        GameLost.Event -= EndGame;
    }
}