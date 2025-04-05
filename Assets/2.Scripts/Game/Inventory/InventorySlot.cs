using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int slotIndex;
    public Image icon;
    public TextMeshProUGUI amountText;

    public void SetSlot(int index, InventoryManager.SaveItem item)
    {
        slotIndex = index;

        if (item.id == 0)
        {
            amountText.text = "";
            Color c;
            if (ColorUtility.TryParseHtmlString("#333333", out c))
            {
                icon.color = c;
            }
        }
        else
        {
            ItemData data = FindAnyObjectByType<InventoryManager>()?.GetItemData(item.id);

            if (data != null)
            {
                icon.sprite = data.icon;
                amountText.text = item.amount.ToString();
                Color c;
                if (ColorUtility.TryParseHtmlString("#FFFFFF", out c))
                {
                    icon.color = c;
                }
            }
        }
    }

    public void OnClick()
    {
        var inventory = FindAnyObjectByType<InventoryManager>();
        if (inventory == null) return;

        var item = inventory.Inventory[slotIndex];
        if (item.id != 0)
        {
            var itemData = inventory.GetItemData(item.id);
            Debug.Log($"클릭한 슬롯 {slotIndex}: {itemData.itemName} x{item.amount}");
            // 여기서 설명창 열기나 아이템 사용 등 처리 가능
        }
    }
}