using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayHeroEquipment : MonoBehaviour
{
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

    HeroModel _currentHero;
    // Start is called before the first frame update
    void Awake()
    {
        _heroSelected.Event += HeroSelectedEvent;
        _equipmentEvent.Event += EquipmentEvent;

        _heroSelected.NotifyEvent(defaultHero);
    }

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
    // Update is called once per frame
    void OnDestroy()
    {
        _heroSelected.Event -= HeroSelectedEvent;
    }
}


