using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Button _equip;
    ItemModel _model;

    //iMAGE SHOULD BE TAKEN FROM SOMEWHERE ELSE
    [SerializeField]
    Image image;

    ItemDetails _itemDetails;

    public void Start()
    {
        _itemDetails = FindObjectOfType<ItemDetails>(true);
    }
    public void SetupButton(ItemModel model)
    {
        //The item model should provide us with the image;
        _model = model;
        _equip.GetComponentInChildren<TMP_Text>().text = model.name;
    }
    public void ResetButton()
    {
        _model = null;
        _equip.GetComponentInChildren<TMP_Text>().text = "";
    }

    public void ActivateButton()
    {
        if(_model != null)
            _itemDetails.SetupDetails(_model);
    }
}
