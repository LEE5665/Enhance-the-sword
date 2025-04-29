using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IdleGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoinText;
    [SerializeField] private int Coin = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void CoinUpdate()
    {
        CoinText.text = "COIN : " + Coin;
    }

    public void AddClickCoin()
    {
        
        Coin += UpgradeManager.Instance.Upgrades["Click"] * 10;
        CoinUpdate();
        Debug.Log("눌렀어");
    }

    public void AddCoin(int Co)
    {
        Coin += Co;
        CoinUpdate();
    }

    public void SubCoin(int Co)
    {
        Coin -= Co;
        CoinUpdate();
    }
}
