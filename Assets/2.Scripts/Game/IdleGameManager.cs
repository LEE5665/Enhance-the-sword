using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoinText;
    InventoryManager Inventory;
    
    void Start()
    {
        Inventory = FindAnyObjectByType<InventoryManager>();
    }

    
    void Update()
    {
    }

    void CoinUpdate()
    {
        Inventory.MoneyDIsplayUpdate();
    }

    public void AddClickCoin()
    {
        Inventory.money += UpgradeManager.Instance.Upgrades["Click"] * 10;
        CoinUpdate();
        Debug.Log("눌렀어");
    }

    public void AddCoin(int Co)
    {
        Inventory.money += Co;
        CoinUpdate();
    }

    public void SubCoin(int Co)
    {
        Inventory.money -= Co;
        CoinUpdate();
    }
}
