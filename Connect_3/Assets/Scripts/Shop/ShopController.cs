using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This class should be divided into smaller classes
public class ShopController : MonoBehaviour
{

    List<ShopItemView> _items = new List<ShopItemView>();
    [SerializeField]
    List<IntEventBus> _boughtEvents = new List<IntEventBus>();
    [SerializeField]
    PlayerProgressionService _playerProgression;
    [SerializeField]
    TMP_Text _coins;
    [SerializeField]
    TMP_Text _gems;
    [SerializeField]
    GameObject _itemPopup;
    [SerializeField]
    ItemGenerator _itemGenerator;

    GameConfigService _config;
    GameAnalyticsService _analytics;
    GameSaveService _progressionProvider;
    private void Awake()
    {
        _items = new List<ShopItemView>(FindObjectsOfType<ShopItemView>());
        foreach(IntEventBus bought in _boughtEvents)
        {
            bought.Event += BoughtEvent;
        }
        _config = ServiceLocator.GetService<GameConfigService>();
        _analytics = ServiceLocator.GetService<GameAnalyticsService>();
        _progressionProvider = ServiceLocator.GetService<GameSaveService>();
    }
    void Start()
    {
        UpdateItems();
    }
    private void OnDestroy()
    {
        foreach (IntEventBus bought in _boughtEvents)
        {
            bought.Event -= BoughtEvent;
        }
    }

    private void BoughtEvent(int index)
    {
        switch (index)
        {
            case 1:
                _playerProgression.ModifyTurnBooster(10);
                _analytics.SendEvent("TurnBoosterBought");
            break;
            case 2:
                _playerProgression.ModifyManaBooster(10);
                _analytics.SendEvent("ManaBoosterBought");
                break;
            case 3:
                _playerProgression.ModifyTileBooster(10);
                _analytics.SendEvent("TileBoosterBought");
                break;
            case 4:
                OpenChest(false);
                _analytics.SendEvent("NormalChestBought");
                break;
            case 5:
                for(int i = 0;i<_config.bigChestItemAmount;i++)
                {
                    OpenChest(false);
                }
                _analytics.SendEvent("BigChestBought");
                break;
            case 6:
                OpenChest(true);
                _analytics.SendEvent("GemsChestBought");
                break;
            case 7:
                _playerProgression.ModifyGems(_config.gemsAddedByAd);
                break;
        }
        _progressionProvider.SaveGame();
        UpdateItems();
    }

    void OpenChest(bool premium)
    {
        ItemModel item = _itemGenerator.GenerateItem(premium);
        Instantiate(_itemPopup,gameObject.transform).GetComponent<ItemDetails>().SetupDetails(item);
    }

    private void UpdateItems()
    {
        foreach (ShopItemView item in _items)
        {
            item.UpdateView();
        }

        _coins.text = _playerProgression.Coins.ToString();
        _gems.text = _playerProgression.Gems.ToString();
    }
}
