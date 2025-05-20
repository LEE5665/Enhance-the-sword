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

    public int InventorySelect = -1;
    public int StoreSelect = -1;
    public TextMeshProUGUI moneyDisplay;
    public Button sellButton;
    public Button useButton;
    public TextMeshProUGUI useButtonText;
    public Button upgradeButton;

    public static InventoryManager Instance { get; private set; }

    private string saveFilePath;

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

        // 먼저, 기존 슬롯 중 같은 아이템이 있는지 확인
        for (int i = 0; i < usableSlotCount; i++)
        {
            if (Inventory[i].id == id)
            {
                Inventory[i].amount += amount;
                RefreshUI();
                return;
            }
        }

        // 빈 슬롯에 새로 추가
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

        // 꽉 차 있으면 추가 불가
        Debug.LogWarning("인벤토리가 가득 찼습니다. 아이템을 추가할 수 없습니다.");
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

        // 업그레이드 슬롯에 추가 시도
        bool added = UpgradeSlotManager.Instance.TryAddItem(new SaveItem { id = selectedItem.id, amount = 1 });
        if (!added)
        {
            UIManager.Instance.ShowNotice("강화 아이템을 중복으로 사용 할 수 없습니다.");
            Debug.LogWarning("업그레이드 슬롯에 추가 실패 (중복 또는 슬롯 없음)");
            return;
        }

        // 인벤토리에서 1개 제거
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
}
