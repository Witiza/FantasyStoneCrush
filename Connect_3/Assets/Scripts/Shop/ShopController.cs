using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This class should be divided into smaller classes
public class ShopController : MonoBehaviour
{
    [SerializeField]
    Vector2Int BigChestVariableAmount;
    [SerializeField]
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
    private void Awake()
    {
        foreach(IntEventBus bought in _boughtEvents)
        {
            bought.Event += BoughtEvent;
        }
    }

    private void BoughtEvent(int index)
    {
        switch (index)
        {
            case 1:
                _playerProgression.TurnBooster.amount++;
            break;
            case 2: 
                _playerProgression.ManaBooster.amount++;
                break;
            case 3:
                _playerProgression.TileBooster.amount++;
                break;
            case 4:
                //normal chest;
                OpenChest(false);
                break;
            case 5:
                //big chest;
                int times = Random.Range(BigChestVariableAmount.x, BigChestVariableAmount.y);
                for(int i = 0;i<times;i++)
                {
                    OpenChest(false);
                }
                break;
            case 6:
                OpenChest(true);
                //gems chest
                break;
        }
        UpdateItems();
    }

    void OpenChest(bool premium)
    {
        ItemModel item = _itemGenerator.GenerateItem(premium);
        Instantiate(_itemPopup,gameObject.transform).GetComponent<ItemDetails>().SetupDetails(item);
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateItems();
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
