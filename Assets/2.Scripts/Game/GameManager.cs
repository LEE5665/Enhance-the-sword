using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int activitePanelIndex = 0;
    public Button option;
    public Button inventory;
    public Button upgrade;
    public Button clicker;
    public GameObject optionPanel;
    public GameObject inventoryPanel;
    public GameObject upgradePanel;
    public GameObject clickerPanel;
    private Image image;
    private Color c;

    public void OnClickIndex(int index)
    {
        activitePanelIndex = index;
        inventoryPanel.SetActive(false);
        upgradePanel.SetActive(false);
        clickerPanel.SetActive(false);

        // 버튼들 색상 초기화 (흰색)
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out c))
        {
            inventory.GetComponent<Image>().color = c;
            upgrade.GetComponent<Image>().color = c;
            clicker.GetComponent<Image>().color = c;
        }

        switch (activitePanelIndex)
        {
            case 1:
                image = inventory.GetComponent<Image>();
                if (ColorUtility.TryParseHtmlString("#656565", out c))
                    image.color = c;
                inventoryPanel.SetActive(true);
                break;
            case 2:
                image = upgrade.GetComponent<Image>();
                if (ColorUtility.TryParseHtmlString("#656565", out c))
                    image.color = c;
                upgradePanel.SetActive(true);
                break;
            case 3:
                image = clicker.GetComponent<Image>();
                if (ColorUtility.TryParseHtmlString("#656565", out c))
                    image.color = c;
                clickerPanel.SetActive(true);
                break;
            default:
                break;
        }
        // 옵션 버튼/패널은 여기서 제어 X (토글 함수로만!)
    }

    // 옵션 패널 토글 함수 + 버튼 색상도 변경
    public void ToggleOptionPanel()
    {
        bool isActive = !optionPanel.activeSelf;
        optionPanel.SetActive(isActive);

        Image optionImage = option.GetComponent<Image>();
        Color activeColor, inactiveColor;
        ColorUtility.TryParseHtmlString("#656565", out activeColor);
        ColorUtility.TryParseHtmlString("#FFFFFF", out inactiveColor);

        optionImage.color = isActive ? activeColor : inactiveColor;
    }
}
