using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject noticeUI;
    public TextMeshProUGUI alarmText;
    private Coroutine currentNoticeCoroutine;
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

    public void ShowNotice(String text)
    {
        if (currentNoticeCoroutine != null)
        {
            // 코루틴이 이미 실행 중이면 초기화
            StopCoroutine(currentNoticeCoroutine);
        }
        alarmText.text = text;
        currentNoticeCoroutine = StartCoroutine(ShowNoticeCoroutine());
    }

private IEnumerator ShowNoticeCoroutine()
{
    noticeUI.SetActive(true);
    noticeUI.transform.localScale = Vector3.one;

    yield return new WaitForSeconds(1f); // 대기 시간

    float duration = 1f;
    float time = 0f;

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = time / duration;

        noticeUI.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
        yield return null;
    }

    noticeUI.SetActive(false);
    noticeUI.transform.localScale = Vector3.one; // 초기화
    currentNoticeCoroutine = null;
}
}
