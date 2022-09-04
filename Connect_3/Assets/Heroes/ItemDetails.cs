using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    [SerializeField]
    EquipmentEvent equipOrUnequipItem;

    [SerializeField]
    TMP_Text _normalMultiplier;
    [SerializeField]
    TMP_Text _criticalMultiplier;
    [SerializeField]
    TMP_Text _manaGainMultiplier;
    [SerializeField]
    TMP_Text _critChance;
    [SerializeField]
    TMP_Text _name;
    [SerializeField]
    TMP_Text _equipButton;
    [SerializeField]
    HeroEventBus _heroSelected;

    HeroModel _currentHero;
    ItemModel _currentItem;


    private void Start()
    {
        if(ItemEquipped())
        {
            _equipButton.text = "Unequip";
        }
        else
        {
            _equipButton.text = "Equip";
        }
    }

    private void HeroSelectedEvent(HeroModel hero)
    {
        _currentHero = hero;    
    }

    public void SetupDetails(ItemModel model)
    {
        gameObject.SetActive(true);
        _currentItem = model;
        Multipliers multi = model.ItemMultipliers;
        _normalMultiplier.text = multi.NormalMultiplier.ToString("F3");
        _criticalMultiplier.text = multi.CriticalMultiplier.ToString("F3");
        _manaGainMultiplier.text = multi.ManaGainMultiplier.ToString("F3");
        _critChance.text = multi.CritChance.ToString("F3");
        _name.text = _currentItem.name;
    }

    bool ItemEquipped()
    {
        return _currentHero.HasItem(_currentItem);
    }
    public void EquipOrUnequip()
    {
        if(ItemEquipped())
        {
            equipOrUnequipItem.NotifyEvent(_currentItem, false);
        }
        else
        {
            equipOrUnequipItem.NotifyEvent(_currentItem, true);
        }
        gameObject.SetActive(false);
    }
    private void Awake()
    {
        _heroSelected.Event += HeroSelectedEvent;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _heroSelected.Event -= HeroSelectedEvent;
    }
}
