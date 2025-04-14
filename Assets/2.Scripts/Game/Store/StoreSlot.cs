using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreSlot : MonoBehaviour
{
    public int slotIndex;
    public Image icon;
    public Image SelectBorder;
    public TextMeshProUGUI amountText;

    private InventoryManager inventory;
    private StoreManager Store;
    public int itemid;

    void Start()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
        Store = FindAnyObjectByType<StoreManager>();
    }

    public void Refresh(bool reset=false)
    {
        ItemData data = inventory?.GetItemData(itemid);
        if (data != null)
        {
            icon.sprite = data.icon;
            amountText.text = "";
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out var c))
                icon.color = c;
        }
        if (inventory?.StoreSelect == slotIndex)
        {
            if (ColorUtility.TryParseHtmlString("#FFFC00", out var c))
                SelectBorder.color = c;
        }
        else
        {
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out var c))
                SelectBorder.color = c;
        }
        if(reset)
        {
                if (ColorUtility.TryParseHtmlString("#FFFFFF", out var c))
                SelectBorder.color = c;
        }
    }

    public void OnClick()
    {
        var itemData = inventory.GetItemData(itemid);
        Debug.Log($"클릭한 슬롯 {slotIndex}: {itemData.itemName}");
        Store.SetDescription(itemData);
        inventory.StoreSelect = slotIndex;
        Store.RefreshStoreUI();
    }
}