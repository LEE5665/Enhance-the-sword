using UnityEngine;
using TMPro;
using System.IO;
using Newtonsoft.Json;

public class UpgradeSlotManager : MonoBehaviour
{
    public static UpgradeSlotManager Instance { get; private set; }

    public InventoryManager.SaveItem[] upgradeSlots = new InventoryManager.SaveItem[3];
    public GameObject[] upgradeSlotUI; 

    private string savePath;

    private int selectedIndex = -1;

    public void SetSelectedIndex(int index)
    {
        selectedIndex = index;
        RefreshUpgradeUI();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "upgradeSlots.json");

        MainMenuSelectManager save = FindAnyObjectByType<MainMenuSelectManager>();
        if (save != null)
        {
            if (save.startState == 0)
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

    public void NewGame()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i] = new InventoryManager.SaveItem { id = 0, amount = 0 };
        }
        RefreshUpgradeUI();
        Debug.Log("업그레이드 슬롯 초기화 완료");
    }

    public void LoadGame()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("업그레이드 슬롯 저장 파일 없음. 새로 생성합니다.");
            NewGame();
            return;
        }

        string json = File.ReadAllText(savePath);
        upgradeSlots = JsonConvert.DeserializeObject<InventoryManager.SaveItem[]>(json);
        RefreshUpgradeUI();
        Debug.Log("업그레이드 슬롯 불러오기 완료");
    }
    public bool TryAddItem(InventoryManager.SaveItem item)
    {
        
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            if (upgradeSlots[i].id == item.id)
            {
                Debug.Log("이미 동일한 아이템이 업그레이드 슬롯에 존재합니다.");
                return false;
            }
        }

        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            if (upgradeSlots[i].id == 0)
            {
                upgradeSlots[i].id = item.id;
                upgradeSlots[i].amount = 1;
                RefreshUpgradeUI();
                return true;
            }
        }

        Debug.Log("모든 업그레이드 슬롯이 가득 찼습니다.");
        return false;
    }

    public void RefreshUpgradeUI()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            var uiSlot = upgradeSlotUI[i].GetComponent<UpgradeSlot>();
            if (uiSlot != null)
            {
                uiSlot.RefreshSlot(); 
                uiSlot.SetSelectBorder(i == selectedIndex);
            }
        }
    }

    public void ClearSelectedIndex()
    {
        selectedIndex = -1;
        RefreshUpgradeUI();
    }
    public void RemoveSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= upgradeSlots.Length)
        {
            Debug.LogWarning("선택된 업그레이드 슬롯이 없습니다.");
            return;
        }

        var slot = upgradeSlots[selectedIndex];
        if (slot.id == 0)
        {
            Debug.LogWarning("선택된 슬롯이 비어 있습니다.");
            return;
        }

        
        InventoryManager.Instance.AddItem(slot.id, 1);

        
        upgradeSlots[selectedIndex].id = 0;
        upgradeSlots[selectedIndex].amount = 0;

        
        selectedIndex = -1;
        RefreshUpgradeUI();

        Debug.Log("업그레이드 슬롯에서 아이템을 해제하고 인벤토리로 반환했습니다.");
    }

    public void ClearAllUpgradeSlots()
    {
        for (int i = 0; i < upgradeSlots.Length; i++)
        {
            upgradeSlots[i].id = 0;
            upgradeSlots[i].amount = 0;
        }
        selectedIndex = -1;
        RefreshUpgradeUI();
        Debug.Log("업그레이드 슬롯 비움 완료");
    }
}
