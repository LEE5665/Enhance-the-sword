using System;
using TMPro;
using UnityEngine;

public class LoadUpgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI LvText;
    [SerializeField] private TextMeshProUGUI BuyText;
    [SerializeField] private GameObject Item;
    [SerializeField] private int DefaultCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (LvText != null)
        {
            if (UpgradeManager.Instance.Upgrades.ContainsKey(gameObject.name))
            {
                LvText.text = $"Lv{UpgradeManager.Instance.Upgrades[gameObject.name]}";
                BuyText.text = $"{(int)(DefaultCost * Math.Pow(UpgradeManager.Instance.Upgrades[gameObject.name], 2))}$";
            }
        }
        else
        {
            if (UpgradeManager.Instance.Upgrades.ContainsKey(gameObject.name))
            {
                Item.SetActive(true);
                Destroy(this);
            }
        }
    }
    public void UpgradeClick(GameObject clickedObject)
    {
        string myName = clickedObject.name;
        int cost = (int)(DefaultCost * Math.Pow(UpgradeManager.Instance.Upgrades[gameObject.name], 2));
        if (InventoryManager.Instance.money >= cost)
        {
            InventoryManager.Instance.money -= cost;
            InventoryManager.Instance.MoneyDIsplayUpdate();
            UpgradeManager.Instance.Upgrades[myName]++;
            Debug.Log($"{myName} 업그레이드 성공! 새 레벨: {UpgradeManager.Instance.Upgrades[myName]}");
            LvText.text = $"Lv{UpgradeManager.Instance.Upgrades[myName]}";
            BuyText.text = $"{(int)(DefaultCost * Math.Pow(UpgradeManager.Instance.Upgrades[gameObject.name], 2))}$";
        } else {
            Debug.Log("돈 부족");
        }
        Debug.Log("내 오브젝트 이름은: " + myName);

        // if (UpgradeManager.Instance.Upgrades.ContainsKey(myName))
        // {
        //     UpgradeManager.Instance.Upgrades[myName]++;
        //     Debug.Log($"{myName} 업그레이드 레벨: {UpgradeManager.Instance.Upgrades[myName]}");
        //     LvText.text = $"Lv{UpgradeManager.Instance.Upgrades[myName]}";
        // }
        //UpgradeManager.Instance.SaveUpgradeData();
    }

    public void BigUpgradeClick(GameObject clickedObject)
    {
        string myName = clickedObject.name;
        int cost = DefaultCost;
        if (InventoryManager.Instance.money >= cost)
        {
            InventoryManager.Instance.money -= cost;
            InventoryManager.Instance.MoneyDIsplayUpdate();
            if (UpgradeManager.Instance.Upgrades.ContainsKey(myName))
            {
                UpgradeManager.Instance.Upgrades[myName]++;
                Debug.Log($"{myName} 업그레이드 레벨: {UpgradeManager.Instance.Upgrades[myName]}");
            }
            else
            {
                UpgradeManager.Instance.Upgrades.Add(myName, 0);
                Debug.Log($"{myName} 새로 등록: {UpgradeManager.Instance.Upgrades[myName]}");

            }
        } else
        {
            Debug.Log("돈 부족");
        }
        Debug.Log("내 오브젝트 이름은: " + myName);
        Item.SetActive(true);
        Destroy(this);
        //UpgradeManager.Instance.SaveUpgradeData();
    }
}
