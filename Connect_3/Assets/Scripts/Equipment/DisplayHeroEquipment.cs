using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHeroEquipment : MonoBehaviour
{
    [SerializeField]
    List<Sprite> portraitList;
    [SerializeField]
    HeroModel defaultHero;
    [SerializeField]
    EquipmentEvent _equipmentEvent;
    [SerializeField]
    HeroEventBus _heroSelected;
    [SerializeField]
    TMP_Text _normalStrenght;
    [SerializeField]
    TMP_Text _criticalStrenght;
    [SerializeField]
    TMP_Text _manaGain;
    [SerializeField]
    TMP_Text _critChance;
    [SerializeField]
    ItemButton[] _heroItems;
    [SerializeField]
    Image _spriteRenderer;

    HeroModel _currentHero;

    private void EquipmentEvent(ItemModel item, bool equipping)
    {
        if(equipping)
        {
            _currentHero.Inventory.EquipItem(item);
        }
        else
        {
            _currentHero.Inventory.UnequipItem(item);
        }
        GenerateHeroDetails();
    }

    private void HeroSelectedEvent(HeroModel hero)
    {
        _currentHero = hero;
        GenerateHeroDetails();
    }

    private void GenerateHeroDetails()
    {
        _currentHero.GenerateFinalStats();
        _critChance.text = _currentHero.CritChance.ToString("F0");
        _normalStrenght.text = _currentHero.NormalStrength.ToString();
        _criticalStrenght.text = _currentHero.CriticalStrength.ToString();
        _manaGain.text = _currentHero.ManaGain.ToString();
        _spriteRenderer.sprite = portraitList[_currentHero.id];

        for (int i = 0; i < _heroItems.Length; i++)
        {
            if (i < _currentHero.Inventory.items.Count)
            {
                _heroItems[i].SetupButton(_currentHero.Inventory.items[i]);
            }
            else
            {
                _heroItems[i].ResetButton();
            }
        }
    }

    void Awake()
    {
        _heroSelected.Event += HeroSelectedEvent;
        _equipmentEvent.Event += EquipmentEvent;
        _heroSelected.NotifyEvent(defaultHero);
    }

    void OnDestroy()
    {
        _heroSelected.Event -= HeroSelectedEvent;
        _equipmentEvent.Event -= EquipmentEvent;
    }
}


