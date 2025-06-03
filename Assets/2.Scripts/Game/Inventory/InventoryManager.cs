using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public List<SaveItem> Inventory;
    public int money;

    public AllItem itemDatabase;

    public GameObject slotPrefab;
    public Transform ParentObject;
    private List<GameObject> slotObjects = new List<GameObject>();
    public TextMeshProUGUI textDescription;
    public TextMeshProUGUI ItemName;

    public int InventorySelect = -1;
    public int StoreSelect = -1;
    public TextMeshProUGUI moneyDisplay;
    public Button sellButton;
    public Button useButton;
    public TextMeshProUGUI useButtonText;
    public Button upgradeButton;

    private List<SaveItem> filteredInventory;
    private bool isFiltered = false;

    public static InventoryManager Instance { get; private set; }

    private string saveFilePath;
    public string currentFilterType = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        MainMenuSelectManager Save = FindAnyObjectByType<MainMenuSelectManager>();
        if (Save != null)
        {
            if (Save.startState == 0)
            {
                NewGame();
            }
            else
            {
                LoadGame();
            }
        }
        else
        {
            NewGame();
        }
    }


    [System.Serializable]
    public class SaveItem
    {
        public int id;
        public int amount;
    }

    public void SaveInventoryData()
    {
        SaveData data = new SaveData
        {
            items = Inventory,
            money = money
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(saveFilePath, json);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("저장 파일 없음");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

        Inventory = data.items;
        money = data.money;
        RefreshUI();
        Debug.Log("불러오기 완료");
    }

    public void NewGame()
    {
        SaveData data = new SaveData
        {
            items = new List<SaveItem>(),
            money = 0
        };

        for (int i = 0; i < 15; i++)
        {
            data.items.Add(new SaveItem
            {
                id = 0,
                amount = 0
            });
        }



        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(saveFilePath, json);

        var inventoryManager = FindAnyObjectByType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.Inventory = data.items;
            inventoryManager.money = data.money;
        }
        Debug.Log("새 게임 시작, 초기화 저장 완료");
    }

    public void AddItem(int id, int amount = 1)
    {
        int usableSlotCount = Inventory.Count;


        for (int i = 0; i < usableSlotCount; i++)
        {
            if (Inventory[i].id == id)
            {
                Inventory[i].amount += amount;
                RefreshUI();
                return;
            }
        }


        for (int i = 0; i < usableSlotCount; i++)
        {
            if (Inventory[i].id == 0)
            {
                Inventory[i].id = id;
                Inventory[i].amount = amount;
                RefreshUI();
                return;
            }
        }


        Debug.LogWarning("인벤토리가 가득 찼습니다. 아이템을 추가할 수 없습니다.");
    }

    public void SetDescription(ItemData data, int n = 0)
    {
        SellButtonOnOff(true);
        ItemName.text = $"{data.itemName}";
        textDescription.text = $"{data.description}\n판매가격 : {data.sell}";
        if (data.type == "upgradeItem")
        {
            textDescription.text += $"\n업그레이드 확률 : {data.upgrade}%";
            textDescription.text += $"\n업그레이드 가격 : {data.upgradeCost}";
        }
    }
    public void SetNullDescription()
    {
        SellButtonOnOff(false);
        ItemName.text = $"";
        textDescription.text = $"아이템 정보가 없습니다.";
    }
    public void AddItemTest()
    {
        AddItem(1, 1);
    }
    public void AddItemTestt()
    {
        for (int i = 200; i <= 208; i++)
        {
            AddItem(i, 1);
        }
    }
    public void AddMoneyTest()
    {
        money += 1000000;
        MoneyDIsplayUpdate();
    }

    public ItemData GetItemData(int id)
    {
        return itemDatabase.GetItemById(id);
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
        MoneyDIsplayUpdate();

    }

    public void MoneyDIsplayUpdate()
    {
        moneyDisplay.text = $"COIN : {money}";
    }

public void RefreshUI()
{
    if (isFiltered && !string.IsNullOrEmpty(currentFilterType))
    {
        // filteredInventory를 항상 새로 만듦
        filteredInventory = new List<SaveItem>();
        foreach (var item in Inventory)
        {
            var itemData = GetItemData(item.id);
            if (item.id != 0 && itemData != null && itemData.type == currentFilterType)
                filteredInventory.Add(new SaveItem { id = item.id, amount = item.amount });
        }
        while (filteredInventory.Count < Inventory.Count)
            filteredInventory.Add(new SaveItem { id = 0, amount = 0 });

        RefreshUI_Filtered();
        return;
    }
    // 필터 아닐 때는 원래대로
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
    public void UseButtonOnOff(bool onoff)
    {
        useButton.gameObject.SetActive(onoff);
        useButtonText.text = "Use";
    }
    public void UpgradeButtonOnOff(bool onoff)
    {
        upgradeButton.interactable = onoff;
    }

    public void SellClick()
    {
        DecreaseSelectedItemAmount(1, true);
    }

    public void DecreaseSelectedItemAmount(int amountToDecrease = 1, bool addMoney = false)
    {
        int index = InventorySelect;

        if (index < 0 || index >= Inventory.Count)
        {
            Debug.LogWarning("선택된 인덱스가 유효하지 않습니다.");
            return;
        }

        var item = Inventory[index];
        if (item.id == 0)
        {
            Debug.Log("빈 슬롯입니다.");
            return;
        }

        var itemData = GetItemData(item.id);
        if (itemData == null)
        {
            Debug.LogWarning("아이템 데이터가 없습니다.");
            return;
        }

        if (addMoney)
        {
            money += itemData.sell * amountToDecrease;
            MoneyDIsplayUpdate();
        }

        item.amount -= amountToDecrease;
        if (item.amount <= 0)
        {
            item.id = 0;
            item.amount = 0;
            SetNullDescription();
        }

        RefreshUI();
        Debug.Log($"{itemData.itemName} {(addMoney ? "판매" : "사용")} 완료. 남은 수량: {item.amount}");
    }

    public void UseSelectedItem()
    {
        if (useButtonText.text == "Remove")
        {
            UpgradeSlotManager.Instance.RemoveSelectedItem();
            UpgradeDescription();
            return;
        }
        if (InventorySelect < 0 || InventorySelect >= Inventory.Count)
        {
            Debug.LogWarning("선택된 인벤토리 슬롯이 없습니다.");
            return;
        }

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


        bool added = UpgradeSlotManager.Instance.TryAddItem(new SaveItem { id = selectedItem.id, amount = 1 });
        if (!added)
        {
            UIManager.Instance.ShowNotice("강화 아이템을 중복으로 사용 할 수 없습니다.");
            Debug.LogWarning("업그레이드 슬롯에 추가 실패 (중복 또는 슬롯 없음)");
            return;
        }


        selectedItem.amount -= 1;
        if (selectedItem.amount <= 0)
        {
            selectedItem.id = 0;
            selectedItem.amount = 0;
            SetNullDescription();
        }
        UpgradeDescription();
        RefreshUI();
        Debug.Log($"{itemData.itemName}를 업그레이드 슬롯으로 이동");
    }
    public void UpgradeDescription()
    {
        string descriptionText = "";
        foreach (var slot in UpgradeSlotManager.Instance.upgradeSlots)
        {
            if (slot.id != 0)
            {
                var data = GetItemData(slot.id);
                if (data != null)
                {
                    descriptionText += data.itemName + "\n";
                }
            }
        }
        UpgradeManager.Instance.SetUpgradeDescription(descriptionText);
    }

    public void SortInventoryById()
    {
        // 아이템 있는 것만 id 기준 정렬 + 빈 슬롯 뒤로
        Inventory.Sort((a, b) =>
        {
            if (a.id == 0 && b.id == 0) return 0;
            if (a.id == 0) return 1;
            if (b.id == 0) return -1;
            return a.id.CompareTo(b.id);
        });
        RefreshUI();
    }

    public void FilterByType(string type)
    {
        filteredInventory = new List<SaveItem>();
        // type에 맞는 아이템만 추출
        foreach (var item in Inventory)
        {
            var itemData = GetItemData(item.id);
            if (item.id != 0 && itemData != null && itemData.type == type)
            {
                filteredInventory.Add(new SaveItem { id = item.id, amount = item.amount });
            }
        }
        // 나머지 칸은 빈칸으로
        while (filteredInventory.Count < Inventory.Count)
        {
            filteredInventory.Add(new SaveItem { id = 0, amount = 0 });
        }
        currentFilterType = type;
    isFiltered = true;
    RefreshUI();
    }

    public void ResetFilter()
    {
        isFiltered = false;
        currentFilterType = "";
        RefreshUI();
    }

    private void RefreshUI_Filtered()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i < slotObjects.Count)
            {
                var slot = slotObjects[i].GetComponent<InventorySlot>();
                slot.SetSlot(i, filteredInventory[i]);
            }
        }
    }
}
