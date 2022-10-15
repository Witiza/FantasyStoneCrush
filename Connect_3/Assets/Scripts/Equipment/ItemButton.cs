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

    public void Start()
    {
        _itemDetails = FindObjectOfType<ItemDetails>(true);
    }

    public void SetupButton(ItemModel model)
    {
        _model = model;
        _icon.sprite = Resources.Load<Sprite>("Icons/" + _model.icon);//ItemGenerator.icons.GetValueOrDefault(_model.icon);
        _icon.color = _model.iconColor;
    }

    public void ResetButton()
    {
        _model = null;
        _icon.sprite = null;
    }

    public void ActivateButton()
    {
        if(_model != null)
            _itemDetails.SetupDetails(_model);
    }
}
