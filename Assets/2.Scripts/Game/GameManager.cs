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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickIndex(int index)
    {
        activitePanelIndex = index;
        //optionPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        upgradePanel.SetActive(false);
        clickerPanel.SetActive(false);
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out c))
        inventory.GetComponent<Image>().color = c;
        upgrade.GetComponent<Image>().color = c;
        clicker.GetComponent<Image>().color = c;
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
    }
}
