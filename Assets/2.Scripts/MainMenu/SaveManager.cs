using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFilePath;
    [SerializeField] private Button LoadButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");

        if (File.Exists(saveFilePath))
        {
            if (LoadButton != null)
                LoadButton.interactable = true;

            Debug.Log("세이브 파일 있음! → 불러오기");
            
        }
        else
        {
            if (LoadButton != null)
                LoadButton.interactable = false;

            Debug.Log("세이브 파일 없음! → 새로 시작");
            
        }
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("❌ 저장 파일 없음");
            NewGame();
            LoadGame();
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData data = JsonConvert.DeserializeObject<SaveData>(json);

        var inventoryManager = FindAnyObjectByType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.Inventory = data.items;
            inventoryManager.money = data.money;
        }
        else
        {
            Debug.LogWarning("❌ InventoryManager를 찾을 수 없습니다");
        }

        Debug.Log("✅ 불러오기 완료");
    }

    public void NewGame()
    {
        SaveData data = new SaveData
        {
            items = new List<InventoryManager.SaveItem>(),
            money = 0
        };

        for (int i = 0; i < 10; i++)
        {
            data.items.Add(new InventoryManager.SaveItem
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
        Debug.Log("✅ 새 게임 시작, 초기화 저장 완료");
    }
}
