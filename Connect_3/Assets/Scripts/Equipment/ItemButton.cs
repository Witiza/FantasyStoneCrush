using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    PlayerInventoryView _inventoryView;

    public void Awake()
    {
        _itemDetails = FindObjectOfType<ItemDetails>(true);
        _inventoryView = FindObjectOfType<PlayerInventoryView>(true);
    }

    public void SetupButton(ItemModel model)
    {
        _model = model;
        StartCoroutine(SetupSpriteCoroutine());
        _icon.color = _model.iconColor;
    }

    public void ResetButton()
    {
        _model = null;
         _icon.sprite = _inventoryView.GetIcon("None");
        _icon.color = Color.white;

    }

    public void ActivateButton()
    {
        if(_model != null)
            _itemDetails.SetupDetails(_model);
    }

    //This is bad, should be an async method that waits for the icons to be loaded
    IEnumerator SetupSpriteCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        _icon.sprite = _inventoryView.GetIcon(_model.icon);
    }
}
