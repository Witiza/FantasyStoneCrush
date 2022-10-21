using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Icon
{
    public Sprite icon;
    public string id;
}
public class PlayerInventoryView : MonoBehaviour
{
    [SerializeField]
    private EquipmentEvent _equipmentEvent;
    [SerializeField]
    private PlayerInventory _inventory;
    [SerializeField]
    private GameObject _buttonTemplate;
    [SerializeField]
    private GridLayoutGroup _itemGrid;
    [SerializeField]
    private RectTransform _viewport;

    [SerializeField]
    private SpritesheetLoader _iconsSheet;
    [SerializeField]
    private SpritesheetLoader _portraitSheet;

    List<GameObject> buttons = new List<GameObject>();

    private void EquipmentEvent(ItemModel item, bool equipping)
    {
        if(equipping)
        {
            _inventory.items.Remove(item);
        }
        else
        {
            _inventory.items.Add(item);
        }
        GenerateInventoryView();
    }

    public Sprite GetIcon(string id)
    {
        return _iconsSheet.getSprite(id);
    }
    public Sprite GetPortrait(string id)
    {
        return _portraitSheet.getSprite(id);
    }
    private void Start()
    {
        float width = _viewport.rect.width- (_itemGrid.padding.left+_itemGrid.padding.right+_itemGrid.spacing.x*(_itemGrid.constraintCount-1));
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
        for (int i = 0; i < _inventory.items.Count; i++)
        {
            GameObject button = Instantiate(_buttonTemplate, _itemGrid.gameObject.transform);
            button.GetComponent<ItemButton>().SetupButton(_inventory.items[i]);
            buttons.Add(button);
        }
    }
    private void Awake()
    {
        _equipmentEvent.Event += EquipmentEvent;
        _portraitSheet.Load();
        _iconsSheet.Load();
    }

    private void OnDestroy()
    {
        _equipmentEvent.Event -= EquipmentEvent;
        _portraitSheet.Unload();
        _iconsSheet.Unload();

    }
}
