using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<SaveItem> Inventory;
    public int money;

    public AllItem itemDatabase;

    public GameObject slotPrefab;
    public Transform ParentObject;
    private List<GameObject> slotObjects = new List<GameObject>();
    public TextMeshProUGUI textDescription;

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

    public void SetDescription(ItemData data)
    {
        textDescription.text = $"{data.description}";
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
}
