using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<SaveItem> Inventory;
    public int money;

    public AllItem itemDatabase;

    public GameObject slotPrefab;
    public Transform ParentObject;
    private List<GameObject> slotObjects = new List<GameObject>();
    public TextMeshProUGUI textDescription;

    public int InventorySelect = -1;
    public int StoreSelect = -1;
    public TextMeshProUGUI moneyDisplay;
    public Button sellButton;


    [System.Serializable]
    public class SaveItem
    {
        public int id;
        public int amount;
    }

    public void AddItem(int id, int amount = 1)
    {
        var item = Inventory.Find(i => i.id == id);
        if (item != null && item.id != 0)
            item.amount += amount;
        else
        {
            var emptySlot = Inventory.Find(i => i.id == 0);
            if (emptySlot != null)
            {
                emptySlot.id = id;
                emptySlot.amount = amount;
            }
        }
        RefreshUI();
    }

    public void SetDescription(ItemData data, int n = 0)
    {
        SellButtonOnOff(true);
        textDescription.text = $"{data.description}\n판매가격 : {data.sell}";
    }
    public void SetNullDescription()
    {
        SellButtonOnOff(false);
        textDescription.text = $"아이템 정보가 없습니다.";
    }
    public void AddItemTest()
    {
        AddItem(1, 1);

    }

    public ItemData GetItemData(int id)
    {
        return itemDatabase.GetItemById(id);
    }


    void Awake()
    {
        SaveManager.Instance.LoadGame();
    }

    void Start()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, ParentObject);
            InventorySlot slot = slotObj.GetComponent<InventorySlot>();
            slot.SetSlot(i, Inventory[i]);
            slotObjects.Add(slotObj);
        }
        moneyDisplay.text = $"COIN : {money}";

    }

    public void RefreshUI()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i < slotObjects.Count)
            {
                InventorySlot slot = slotObjects[i].GetComponent<InventorySlot>();
                slot.SetSlot(i, Inventory[i]);
            }
        }
    }
    public void RefreshStoreUI()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i < slotObjects.Count)
            {
                InventorySlot slot = slotObjects[i].GetComponent<InventorySlot>();
                slot.SetSlot(i, Inventory[i]);
            }
        }
    }
    public void SellButtonOnOff(bool onoff)
    {
        sellButton.gameObject.SetActive(onoff);
    }

    public void SellSelectedItem()
{
    var selectedItem = Inventory[InventorySelect];

    if (selectedItem.id == 0)
    {
        Debug.Log("빈 슬롯입니다.");
        return;
    }

    var itemData = GetItemData(selectedItem.id);
    if (itemData == null)
    {
        Debug.LogWarning("아이템 데이터가 없습니다.");
        return;
    }

    money += itemData.sell;
    selectedItem.amount -= 1;

    if (selectedItem.amount <= 0)
    {
        SetNullDescription();
        selectedItem.id = 0;
        selectedItem.amount = 0;
    }
    moneyDisplay.text = $"COIN : {money}";
    RefreshUI();
     // 설명 초기화 또는 필요 시 유지
    Debug.Log($"{itemData.itemName} 판매 완료. 남은 수량: {selectedItem.amount}, 현재 소지금: {money}");
}
}
