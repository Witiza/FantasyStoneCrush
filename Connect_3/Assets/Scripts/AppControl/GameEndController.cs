using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndController : MonoBehaviour
{
    public PlayerProgressionService PlayerProgression;
   
    float MovesMultiplier;
    float LowerLevelMultiplier;
    public GameEndEventBus GameWon;
    public GameEndEventBus GameLost;
    public StringEventBus LevelEvent;
    public IntEventBus CoinsWon;
    bool _gameEnded = false;

    public GameObject WonGO;
    public GameObject LostGO;

    GameAnalyticsService _analytics;

    void EndGame(GameEndInfo info)
    {
        if (!_gameEnded)
        {
            _gameEnded = true;
            gameObject.SetActive(true);
            if (info._gameWon)
            {
                _analytics.SendEvent("LevelWon", new Dictionary<string, object> { ["CurrentLevel"] = info._level });
                WonGO.SetActive(true);
                if (PlayerProgression.CurrentLevel < PlayerProgression.levels.Count)
                {
                    if (PlayerProgression.CurrentLevel == PlayerProgression.MaxLevelUnlocked)
                    {
                        PlayerProgression.MaxLevelUnlocked++;
                    }
                    PlayerProgression.CurrentLevel++;
                }
            }
            else
            {
                _analytics.SendEvent("LevelFailed", new Dictionary<string, object> { ["CurrentLevel"] = info._level });
                LostGO.SetActive(true);
            }
            CalculateEndCoins(info._remainingMoves, info._score, info._level, info._highestLevel);
        }
    }

    void CalculateEndCoins(int moves,int score, int level, int maxLevel)
    {
        int amount = Mathf.RoundToInt(moves * MovesMultiplier);
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
        PlayerProgression.ModifyCoins(amount); 
    }

    private void Awake()
    {
        GameWon.Event += EndGame;
        GameLost.Event += EndGame;
        gameObject.SetActive(false);

       //amount/(LowerLevelMultiplier*(maxlvl-lvl))

        _analytics = ServiceLocator.GetService<GameAnalyticsService>();
        GameConfigService config = ServiceLocator.GetService<GameConfigService>();
        MovesMultiplier = config.coinsWonMultiplier;
        LowerLevelMultiplier = config.coinsWonMultiplierLowLevel;
    }
    private void OnDestroy()
    {
        GameWon.Event -= EndGame;
        GameLost.Event -= EndGame;
    }
}
