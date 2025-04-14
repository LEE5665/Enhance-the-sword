using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private Transform storeSlotParent;
    private StoreSlot[] StoreSlots;
    public TextMeshProUGUI textDescription;

    void Start()
    {
        StoreSlots = storeSlotParent.GetComponentsInChildren<StoreSlot>();
        RefreshStoreUI();
    }
    public void RefreshStoreUI()
    {
        for (int i = 0; i < StoreSlots.Length; i++)
        {
            StoreSlots[i].slotIndex = i;
            StoreSlots[i].Refresh();
        }
    }

    public void SetDescription(ItemData data)
    {
        textDescription.text = $"{data.description}\n판매가격 : {data.sell}";
    }
    public void SetNullDescription()
    {
        textDescription.text = $"아이템 정보가 없습니다.";
    }
}
