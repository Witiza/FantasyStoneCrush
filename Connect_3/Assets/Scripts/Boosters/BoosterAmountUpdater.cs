using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoosterAmountUpdater : MonoBehaviour
{
    [SerializeField]
    private Booster _booster;
    private TextMeshProUGUI _amount;

    private void Awake()
    {
        _booster.BoosterEvent += BoosterEvent;
        _amount = GetComponent<TextMeshProUGUI>();
        _amount.text = _booster.amount.ToString();
    }

    private void BoosterEvent(bool success)
    {
        _amount.text = _booster.amount.ToString();
    }
}
