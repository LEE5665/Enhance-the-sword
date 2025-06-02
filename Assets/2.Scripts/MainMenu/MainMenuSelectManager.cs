using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class MainMenuSelectManager : MonoBehaviour
{
    public static MainMenuSelectManager Instance { get; private set; }
    private string saveFilePath;
    public int startState = 0; 
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
        }
        else
        {
            if (LoadButton != null)
                LoadButton.interactable = false;
        }
    }
    public void SceneMove()
    {
        SceneManager.LoadScene(1);
    }

    public void NewGame()
    {
        startState = 0;
        SceneMove();
    }
    public void LoadGame()
    {
        startState = 1;
        SceneMove();
    }
}
