using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeSlot : MonoBehaviour
{
    public int slotIndex;
    public Image icon;
    public Image selectBorder;
    public TextMeshProUGUI amountText;

    private void Start()
    {
        RefreshSlot();
    }

    public void RefreshSlot()
    {
        var slot = UpgradeSlotManager.Instance.upgradeSlots[slotIndex];

        if (slot.id == 0)
        {
            icon.sprite = null;
            amountText.text = "";
            SetColor("#333333");
        }
        else
        {
            var itemData = InventoryManager.Instance.GetItemData(slot.id);
            if (itemData != null)
            {
                icon.sprite = itemData.icon;
                amountText.text = "1";
                SetColor("#FFFFFF");
            }
        }

        SetSelectBorder(false); // 기본은 선택 안된 상태
    }

    public void OnClick()
    {
        InventoryManager.Instance.InventorySelect = -1;
        InventoryManager.Instance.RefreshUI();
        var slot = UpgradeSlotManager.Instance.upgradeSlots[slotIndex];
    if (slot.id != 0)
    {
        var itemData = InventoryManager.Instance.GetItemData(slot.id);
        Debug.Log($"업그레이드 슬롯 {slotIndex} 클릭: {itemData.itemName} x{slot.amount}");
        InventoryManager.Instance.SetDescription(itemData);

        if (itemData.type == "use")
        {
            InventoryManager.Instance.UseButtonOnOff(true);
            InventoryManager.Instance.useButtonText.text = "Remove";
        }
        else
        {
            InventoryManager.Instance.UseButtonOnOff(false);
        }
    }
    else
    {
        InventoryManager.Instance.SetNullDescription();
        InventoryManager.Instance.UseButtonOnOff(false);
        Debug.Log($"업그레이드 슬롯 {slotIndex} 클릭: 비어 있음");
    }

        // 선택 테두리 갱신
        UpgradeSlotManager.Instance.SetSelectedIndex(slotIndex);
    }

    private void SetColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color c))
        {
            icon.color = c;
        }
    }

    public void SetSelectBorder(bool on)
    {
        if (on)
        {
            SetBorderColor("#FFFC00");
        }
        else
        {
            SetBorderColor("#FFFFFF");
        }
    }

    private void SetBorderColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color c))
        {
            selectBorder.color = c;
        }
    }
}
