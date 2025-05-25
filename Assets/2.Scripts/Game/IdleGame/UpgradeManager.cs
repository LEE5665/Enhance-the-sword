using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public TextMeshProUGUI upgradeDescription;
    [System.Serializable]
    public class UpgradeData
    {
        public Dictionary<string, int> upgrades = new Dictionary<string, int>();
    }

    public static UpgradeManager Instance { get; private set; }

    private string upgradeSavePath;
    public Dictionary<string, int> Upgrades = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        upgradeSavePath = Path.Combine(Application.persistentDataPath, "upgrade_save.json");

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

    private void LoadGame()
    {
        if (File.Exists(upgradeSavePath))
        {
            string json = File.ReadAllText(upgradeSavePath);
            UpgradeData data = JsonConvert.DeserializeObject<UpgradeData>(json);
            Upgrades = data.upgrades;
            Debug.Log("업그레이드 데이터 불러옴");
        }
        else
        {
            Debug.LogWarning("업그레이드 데이터가 없어서 로드 실패. NewGame()을 호출하세요.");
        }
    }

    private void NewGame()
    {
        // 기본값 세팅
        Upgrades = new Dictionary<string, int>
    {
        { "Click", 1 },
    };
        SaveUpgradeData();
        Debug.Log("업그레이드 데이터 새로 생성");
    }

    public void SaveUpgradeData()
    {
        UpgradeData data = new UpgradeData { upgrades = Upgrades };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(upgradeSavePath, json);
    }

    public void UpgradeClick(GameObject clickedObject)
    {
        string myName = clickedObject.name;
        Debug.Log("내 오브젝트 이름은: " + myName);

        if (Upgrades.ContainsKey(myName))
        {
            Upgrades[myName]++;
            Debug.Log($"{myName} 업그레이드 레벨: {Upgrades[myName]}.");
        }
        else
        {
            Upgrades.Add(myName, 0);
            Debug.Log($"{myName} 새로 등록: {Upgrades[myName]}");
        }

        SaveUpgradeData();
    }

    public void SetUpgradeDescription(string text)
    {
        upgradeDescription.text = text;
    }

    public void ItemUpgradeClick()
    {
        int selectedIndex = InventoryManager.Instance.InventorySelect;

        if (selectedIndex < 0 || selectedIndex >= InventoryManager.Instance.Inventory.Count)
        {
            Debug.LogWarning("선택된 슬롯이 없습니다.");
            return;
        }

        var selectedItem = InventoryManager.Instance.Inventory[selectedIndex];
        if (selectedItem.id == 0)
        {
            Debug.LogWarning("빈 슬롯입니다.");
            return;
        }

        int currentId = selectedItem.id;
        var itemData = InventoryManager.Instance.GetItemData(currentId);

        if (itemData == null)
        {
            Debug.LogWarning("아이템 데이터가 없습니다.");
            return;
        }

        int upgradeCost = itemData.upgradeCost;

        if (InventoryManager.Instance.money < upgradeCost)
        {
            Debug.LogWarning($"소지금 부족. 업그레이드 비용: {upgradeCost}, 현재 소지금: {InventoryManager.Instance.money}");
            UIManager.Instance.ShowNotice("소지금이 부족합니다.");
            return;
        }

        // 소지금 차감
        InventoryManager.Instance.money -= upgradeCost;
        InventoryManager.Instance.MoneyDIsplayUpdate();

        int baseRate = itemData.upgrade;
        int bonusRate = UpgradeBonus(); // 아래 함수 참조
        int finalRate = baseRate + bonusRate;

        int rand = Random.Range(0, 100);

        if (rand < finalRate)
        {
            InventoryManager.Instance.AddItem(currentId + 1, 1);
            Debug.Log($"업그레이드 성공! +{bonusRate}% 보너스 포함 최종 {finalRate}%. {currentId + 1} 아이템 추가");
        }
        else
        {
            Debug.Log($"업그레이드 실패! (기본 {baseRate}% + 보너스 {bonusRate}%) → 최종 확률 {finalRate}%, 주사위: {rand}");
        }

        // 슬롯 제거
        UpgradeSlotManager.Instance.ClearAllUpgradeSlots();

        // 선택 해제 및 UI 갱신
        InventoryManager.Instance.InventorySelect = -1;
        InventoryManager.Instance.SetNullDescription();
        InventoryManager.Instance.RefreshUI();
    }
    private int UpgradeBonus()
    {
        int bonus = 0;
        foreach (var slot in UpgradeSlotManager.Instance.upgradeSlots)
        {
            if (slot.id == 101)
            {
                bonus += 10;
            }
            if (slot.id == 102)
            {
                bonus += 20;
            }
            if (slot.id == 103)
            {
                bonus += 30;
            }
        }
        return bonus;
    }
}
