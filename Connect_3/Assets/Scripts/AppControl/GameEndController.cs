using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameEndController : MonoBehaviour
{
    public PlayerProgressionService PlayerProgression;
   
    float MovesMultiplier;
    float LowerLevelMultiplier;
    public GameEndEventBus GameWon;
    public GameEndEventBus GameLost;
    public EventBus MovesAdded;
    public StringEventBus LevelEvent;
    public IntEventBus CoinsWon;
    bool _gameEnded = false;
    bool _bonusMovesUsed = false;

    public GameObject WonGO;
    public GameObject LostGO;
    public GameObject AddMovesGO;
    public Button NextLevelButton;

    GameAnalyticsService _analytics;
    GameAdsService _ads;
    GameLevelsService _levels;

    void EndGame(GameEndInfo info)
    {
        if (!_gameEnded)
        {
            _gameEnded = true;
            gameObject.SetActive(true);
            if (info._gameWon)
            {
                NextLevelButton.interactable = true;
                _analytics.SendEvent("LevelWon", new Dictionary<string, object> { ["CurrentLevel"] = info._level });
                WonGO.SetActive(true);
                if (PlayerProgression.CurrentLevel < _levels.levels.Count-1)
                {
                    if (PlayerProgression.CurrentLevel == PlayerProgression.MaxLevelUnlocked)
                    {
                        PlayerProgression.MaxLevelUnlocked++;
                    }
                    PlayerProgression.CurrentLevel++;
                }
                else
                {
                    NextLevelButton.interactable = false;
                }
                CalculateEndCoins(info._remainingMoves, info._score, info._level, info._highestLevel);
            }
            else
            {
                LostGO.SetActive(true);
                if(!_bonusMovesUsed)
                {
                    AddMovesGO.SetActive(true);
                }
            }
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
        LostGO.SetActive(false);
        WonGO.SetActive(false);
        _analytics = ServiceLocator.GetService<GameAnalyticsService>();
        _ads = ServiceLocator.GetService<GameAdsService>();
        _levels = ServiceLocator.GetService<GameLevelsService>();
        GameConfigService config = ServiceLocator.GetService<GameConfigService>();
        MovesMultiplier = config.coinsWonMultiplier;
        LowerLevelMultiplier = config.coinsWonMultiplierLowLevel;
    }

    public async void MovesAd()
    {
        if(await _ads.ShowAd())
        {
            MovesAdded.NotifyEvent();
            gameObject.SetActive(false);
            _gameEnded = false;
            _bonusMovesUsed = true;
            AddMovesGO.SetActive(false);
            LostGO.SetActive(false);
        }
    }

    public void ConcludeLostLevel()
    {
        _analytics.SendEvent("LevelFailed", new Dictionary<string, object> { ["CurrentLevel"] = PlayerProgression.CurrentLevel });
    }

    private void OnDestroy()
    {
        GameWon.Event -= EndGame;
        GameLost.Event -= EndGame;
    }
}
