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

    void Start()
    {
        UpdateLevelSelector();
    }

    void UpdateLevelSelector()
    {
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
