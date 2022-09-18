using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovesAdViewer : MonoBehaviour
{
    [SerializeField] Button _adButton;
    [SerializeField] TMP_Text _text;

    void OnEnable()
    {
        _adButton.interactable = ServiceLocator.GetService<GameAdsService>().IsAdReady;
        _text.text = ServiceLocator.GetService<GameConfigService>().movesAddedByAd.ToString() + " Moves";
    }
}
