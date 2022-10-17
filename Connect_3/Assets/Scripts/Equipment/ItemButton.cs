using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Button _equip;
    [SerializeField]
    Image _icon;
    ItemModel _model;
    ItemDetails _itemDetails;
    [SerializeField]
    PlayerInventoryView _inventoryView;

    public void Start()
    {
        _itemDetails = FindObjectOfType<ItemDetails>(true);
        _inventoryView = FindObjectOfType<PlayerInventoryView>(true);
    }

    public void SetupButton(ItemModel model)
    {
        _model = model;
        Sprite tmp = _inventoryView.getIcon(_model.icon);
        _icon.sprite = tmp;
        _icon.color = _model.iconColor;
    }

    public void ResetButton()
    {
        _model = null;
        _icon.sprite = _inventoryView.getIcon("None");
    }

    public void ActivateButton()
    {
        if(_model != null)
            _itemDetails.SetupDetails(_model);
    }
}
