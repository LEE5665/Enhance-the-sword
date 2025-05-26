using TMPro;
using UnityEngine;
using System.Collections;

public class StoreManager : MonoBehaviour
{
    [SerializeField] public Transform storeSlotParent;
    public StoreSlot[] StoreSlots;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI textDescription;

    // IEnumerator Start()
    // {
    //     yield return null;
    //     StoreSlots = storeSlotParent.GetComponentsInChildren<StoreSlot>(true);
    //     Debug.Log("있나?");
    //     RefreshStoreUI(true);
    // }
    public void RefreshStoreUI(bool reset = false)
    {
        for (int i = 0; i < StoreSlots.Length; i++)
        {
            StoreSlots[i].slotIndex = i;
            StoreSlots[i].Refresh(reset);
        }
    }

    public void SetDescription(ItemData data)
    {
        itemName.text = data.itemName;
        textDescription.text = $"{data.description}\n구매가격 : {data.sell}";
    }
    public void SetNullDescription()
    {
        itemName.text = "";
        textDescription.text = $"아이템 정보가 없습니다.";
    }

    public void BuySelectedItem()
    {
        var inventory = FindAnyObjectByType<InventoryManager>();

        if (inventory == null)
        {
            Debug.LogWarning("InventoryManager를 찾을 수 없습니다.");
            return;
        }

        if (inventory.StoreSelect < 0)
        {
            Debug.Log("선택된 상점 슬롯이 없습니다.");
            return;
        }

        StoreSlot selectedSlot = null;

        for (int i = 0; i < StoreSlots.Length; i++)
        {
            if (StoreSlots[i].slotIndex == inventory.StoreSelect)
            {
                selectedSlot = StoreSlots[i];
                break;
            }
        }

        if (selectedSlot == null)
        {
            Debug.LogWarning("선택된 슬롯이 유효하지 않습니다.");
            return;
        }

        var itemData = inventory.GetItemData(selectedSlot.itemid);
        if (itemData == null)
        {
            Debug.LogWarning("해당 ID의 아이템 데이터를 찾을 수 없습니다.");
            return;
        }

        if (inventory.money < itemData.sell)
        {
            UIManager.Instance.ShowNotice("돈이 부족합니다.");
            Debug.Log("돈이 부족합니다!");
            return;
        }
        inventory.AddItem(selectedSlot.itemid, 1);
        inventory.money -= itemData.sell;
        inventory.moneyDisplay.text = $"COIN : {inventory.money}";

        Debug.Log($"{itemData.itemName} 구입 완료! 현재 소지금: {inventory.money}");
    }
}
