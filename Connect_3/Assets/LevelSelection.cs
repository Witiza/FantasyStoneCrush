using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] TMP_Text _levelText;
    [SerializeField] Button _previousButton;
    [SerializeField] Button _nextButton;
    [SerializeField] PlayerProgressionService _progression;
    GameLevelsService _gameLevelsService;
    int _maxLevel;

    void Start()
    {
        _gameLevelsService = ServiceLocator.GetService<GameLevelsService>();

        UpdateLevelSelector();
    }

    void UpdateLevelSelector()
    {
        CheckMaxLevelConsistency();
        if (_progression.CurrentLevel == 0)
        {
            _levelText.text = "Tutorial";
            _previousButton.interactable = false;
        }
        else
        {
            _levelText.text = "Level\n"+_progression.CurrentLevel.ToString();
            _previousButton.interactable = true;

        }
        if (_progression.CurrentLevel < _progression.MaxLevelUnlocked)
        {
            _nextButton.interactable = true;
        }
        else
        {
            _nextButton.interactable = false;
        }
    }

    void CheckMaxLevelConsistency()
    {
        if(_progression.MaxLevelUnlocked > _gameLevelsService.levels.Count)
        {
            _maxLevel = _gameLevelsService.levels.Count;
        }
        else
        {
            _maxLevel = _progression.MaxLevelUnlocked;
        }
    }
    public void SelectNextLevel()
    {
        _progression.CurrentLevel++;
        UpdateLevelSelector();
    }

    public void SelectPreviousLevel()
    {
         _progression.CurrentLevel--;
        UpdateLevelSelector();
    }
}
