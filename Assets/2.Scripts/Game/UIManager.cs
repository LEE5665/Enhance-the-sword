using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject noticeUI;
    public GameObject noticePanel;
    public TextMeshProUGUI alarmText;
    public TextMeshProUGUI alarmTitleText;
    private Coroutine currentNoticeCoroutine;
    private Tween currentTween;
    public static UIManager Instance { get; private set; }

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

    
    public void ShowNotice(string text, string title = "알림", Color? color = null)
    {
        Color panelColor = color ?? Color.red;

        
        currentTween?.Kill();
        noticeUI.transform.DOKill();

        
        alarmTitleText.text = title;
        alarmText.text = text;
        
        var image = noticePanel.GetComponent<Image>();
        if (image != null) image.color = panelColor;

        noticeUI.SetActive(true);
        noticeUI.transform.localScale = Vector3.zero;

        
        noticeUI.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            
            currentTween = DOVirtual.DelayedCall(1f, () =>
            {
                noticeUI.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                {
                    noticeUI.SetActive(false);
                    noticeUI.transform.localScale = Vector3.one; 
                });
            });
        });
    }
}
