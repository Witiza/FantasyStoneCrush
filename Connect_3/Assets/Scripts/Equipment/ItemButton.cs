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
    TMP_Text _buttonText;
    ItemModel _model;
    ItemDetails _itemDetails;

    public void Start()
    {
        _itemDetails = FindObjectOfType<ItemDetails>(true);
    }

    public void SetupButton(ItemModel model)
    {
        _model = model;
        _buttonText.text = model.name;
    }

    public void ResetButton()
    {
        _model = null;
        _buttonText.text = "";
    }

    public void ActivateButton()
    {
        if(_model != null)
            _itemDetails.SetupDetails(_model);
    }
}
