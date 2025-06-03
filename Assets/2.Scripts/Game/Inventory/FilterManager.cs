using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
    public static FilterManager Instance { get; private set; }
    public List<Image> filterIcons;
    public List<string> filterTypes; // 각 버튼에 대응하는 type명. 전체 버튼(인벤토리)는 빈 문자열 ""로!
    public Color32 selectedColor = new Color32(145, 145, 145, 255); // 어두운 회색

    public Color defaultColor = Color.white;

    private int selectedFilterIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Color c;
        if (ColorUtility.TryParseHtmlString("#828282", out c))
        {
            selectedColor = c;
        }
    }

    public void SelectFilter(int idx)
    {
        selectedFilterIndex = idx;
        // 색상 처리
        for (int i = 0; i < filterIcons.Count; i++)
        {
            filterIcons[i].color = (i == idx) ? selectedColor : defaultColor;
        }
        // 동작 연결
        if (string.IsNullOrEmpty(filterTypes[idx]))
        {
            InventoryManager.Instance.ResetFilter();
        }
        else
        {
            InventoryManager.Instance.FilterByType(filterTypes[idx]);
        }
    }
}
