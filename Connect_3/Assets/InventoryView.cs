using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory inventory;
    [SerializeField]
    private GameObject _buttonTemplate;
    [SerializeField]
    private GridLayoutGroup _itemGrid;
    [SerializeField]
    private RectTransform viewport;

    private void Awake()
    {
    }
    private void Start()
    {
        float width = viewport.rect.width- (_itemGrid.padding.left+_itemGrid.padding.right+_itemGrid.spacing.x*(_itemGrid.constraintCount-1));
        float final_width = width / _itemGrid.constraintCount;
        Vector2 newSize = new Vector2(final_width, final_width);
        _itemGrid.cellSize = newSize;
        StartCoroutine(InverntoryCoroutine());
    }

    void GenerateInventoryView()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject button = Instantiate(_buttonTemplate, _itemGrid.gameObject.transform);
        }
    }

    IEnumerator InverntoryCoroutine()
    {
        yield return new WaitForSeconds(1f);
        GenerateInventoryView();
    }
}
