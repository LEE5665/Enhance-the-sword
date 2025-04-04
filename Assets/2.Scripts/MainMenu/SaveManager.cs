using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;
    [SerializeField] private Button StartButton;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(saveFilePath))
        {
            StartButton.interactable = false;
            Debug.Log("세이브 파일 있음! → 불러오기");
            //LoadGame();
        }
        else
        {
            StartButton.interactable = true;
            Debug.Log("세이브 파일 없음! → 새로 시작");
            //NewGame();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadGame()
    {
        string json = File.ReadAllText(saveFilePath);
    }

    void NewGame()
    {
        // 새 게임 데이터 생성 및 저장
        // 예: PlayerPrefs 초기화, 기본 스탯 설정 등
    }
}
