using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using TMPro;

public class Upgrade : MonoBehaviour
{

    public TextMeshProUGUI LvText;
    public TextMeshProUGUI BuyText;

    public void UpgradeClick(GameObject clickedObject)
    {
        string myName = clickedObject.name;
        Debug.Log("내 오브젝트 이름은: " + myName);

        if (UpgradeManager.Instance.Upgrades.ContainsKey(myName))
        {
            UpgradeManager.Instance.Upgrades[myName]++;
            Debug.Log($"{myName} 업그레이드 레벨: {UpgradeManager.Instance.Upgrades[myName]}");
            LvText.text = $"Lv{UpgradeManager.Instance.Upgrades[myName]}";
        }
        else
        {
            UpgradeManager.Instance.Upgrades.Add(myName, 1);
            Debug.Log($"{myName} 새로 등록: {UpgradeManager.Instance.Upgrades[myName]}");
        }

        //UpgradeManager.Instance.SaveUpgradeData();
    }

    public void BigUpgradeClick(GameObject clickedObject)
    {
        string myName = clickedObject.name;
        Debug.Log("내 오브젝트 이름은: " + myName);

        if (UpgradeManager.Instance.Upgrades.ContainsKey(myName))
        {
            UpgradeManager.Instance.Upgrades[myName]++;
            Debug.Log($"{myName} 업그레이드 레벨: {UpgradeManager.Instance.Upgrades[myName]}");
            LvText.text = $"Lv{UpgradeManager.Instance.Upgrades[myName]}";
        }
        else
        {
            UpgradeManager.Instance.Upgrades.Add(myName, 1);
            Debug.Log($"{myName} 새로 등록: {UpgradeManager.Instance.Upgrades[myName]}");
        }

        //UpgradeManager.Instance.SaveUpgradeData();
    }
}
