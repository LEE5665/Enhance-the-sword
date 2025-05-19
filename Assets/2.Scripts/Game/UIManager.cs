using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject noticeUI;
    public TextMeshProUGUI alarmText;
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
public void ShowNotice(string text)
{
    // 트윈 중복 제거
    currentTween?.Kill();
    noticeUI.transform.DOKill(); // 이전 scale 애니메이션도 제거

    // 초기 설정
    alarmText.text = text;
    noticeUI.SetActive(true);
    noticeUI.transform.localScale = Vector3.zero;

    // 부드럽게 커지면서 등장
    noticeUI.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
    {
        // 1초 유지 후 작아지며 사라짐
        currentTween = DOVirtual.DelayedCall(1f, () =>
        {
            noticeUI.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                noticeUI.SetActive(false);
                noticeUI.transform.localScale = Vector3.one; // 초기화 (다음 표시 대비)
            });
        });
    });
}
}
