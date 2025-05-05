using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class UpgradeManager : MonoBehaviour
{
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
            if (Save.startState == 0) {
                NewGame();
            } else {
                LoadGame();
            }
        } else {
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
        { "Miner", 0 }
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
            Upgrades.Add(myName, 1);
            Debug.Log($"{myName} 새로 등록: {Upgrades[myName]}");
        }

        SaveUpgradeData();
    }
}
