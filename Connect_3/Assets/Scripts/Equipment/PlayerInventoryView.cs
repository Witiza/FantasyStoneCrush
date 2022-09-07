using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryView : MonoBehaviour
{
    [SerializeField]
    private EquipmentEvent _equipmentEvent;
    [SerializeField]
    private PlayerInventory inventory;
    [SerializeField]
    private GameObject _buttonTemplate;
    [SerializeField]
    private GridLayoutGroup _itemGrid;
    [SerializeField]
    private RectTransform viewport;

    List<GameObject> buttons = new List<GameObject>();


    private void EquipmentEvent(ItemModel item, bool equipping)
    {
        if(equipping)
        {
            inventory.items.Remove(item);
        }
        else
        {
            inventory.items.Add(item);
        }
        GenerateInventoryView();
    }

    private void Start()
    {
        float width = viewport.rect.width- (_itemGrid.padding.left+_itemGrid.padding.right+_itemGrid.spacing.x*(_itemGrid.constraintCount-1));
        float final_width = width / _itemGrid.constraintCount;
        Vector2 newSize = new Vector2(final_width, final_width);
        _itemGrid.cellSize = newSize;
        GenerateInventoryView();
    }

    void GenerateInventoryView()
    {
        foreach(GameObject button in buttons)
        {
           Destroy(button);
        }
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject button = Instantiate(_buttonTemplate, _itemGrid.gameObject.transform);
            button.GetComponent<ItemButton>().SetupButton(inventory.items[i]);
            buttons.Add(button);
        }
    }
    private void Awake()
    {
        _equipmentEvent.Event += EquipmentEvent;
    }

    private void OnDestroy()
    {
        _equipmentEvent.Event -= EquipmentEvent;
    }
}
