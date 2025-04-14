using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int slotIndex;
    public Image icon;
    public Image SelectBorder;
    public TextMeshProUGUI amountText;

    private InventoryManager inventory;
    private GameObject ghostSlot;
    private int draggedIndex = -1;
    private int itemid;

    void Start()
    {
        inventory = FindAnyObjectByType<InventoryManager>();
    }

    public void SetSlot(int index, InventoryManager.SaveItem item)
    {
        slotIndex = index;

        if (item.id == 0)
        {
            amountText.text = "";
            icon.sprite = null;
            Color c;
            if (ColorUtility.TryParseHtmlString("#333333", out c))
            {
                icon.color = c;
            }
        }
        else
        {
            ItemData data = inventory?.GetItemData(item.id);

            if (data != null)
            {
                itemid = item.id;
                icon.sprite = data.icon;
                amountText.text = item.amount.ToString();
                Color c;
                if (ColorUtility.TryParseHtmlString("#FFFFFF", out c))
                {
                    icon.color = c;
                }
            }
        }
        if (inventory?.InventorySelect == index)
        {
            Color c;
            if (ColorUtility.TryParseHtmlString("#FFFC00", out c))
            {
                SelectBorder.color = c;
            }
        }
        else
        {
            Color c;
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out c))
            {
                SelectBorder.color = c;
            }
        }
    }

    public void OnClick()
    {
        if (inventory == null) return;

        var item = inventory.Inventory[slotIndex];
        if (item.id != 0)
        {
            var itemData = inventory.GetItemData(item.id);
            Debug.Log($"클릭한 슬롯 {slotIndex}: {itemData.itemName} x{item.amount}");
            inventory.SetDescription(itemData);
            inventory.InventorySelect = slotIndex;
            inventory.RefreshUI();
        }
        else
        {
            inventory.InventorySelect = slotIndex;
            inventory.SetNullDescription();
            inventory.RefreshUI();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventory == null || icon.sprite == null) return;

        // 드래그용 슬롯 복제
        ghostSlot = Instantiate(gameObject, transform.root);
        ghostSlot.transform.SetAsLastSibling();

        CanvasGroup cg = ghostSlot.GetComponent<CanvasGroup>();
        if (cg == null) cg = ghostSlot.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;

        draggedIndex = slotIndex;
        inventory.SetDescription(inventory.GetItemData(itemid));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostSlot != null)
        {
            ghostSlot.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ghostSlot != null)
        {
            Destroy(ghostSlot);
            ghostSlot = null;
        }
        draggedIndex = -1;
    }

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlot draggedSlot = eventData.pointerDrag?.GetComponent<InventorySlot>();

        if (draggedSlot != null && draggedSlot.draggedIndex != -1 && draggedSlot.draggedIndex != slotIndex)
        {
            var inventory = FindAnyObjectByType<InventoryManager>();
            if (inventory != null)
            {
                // 🔁 아이템 스왑
                var temp = inventory.Inventory[slotIndex];
                inventory.Inventory[slotIndex] = inventory.Inventory[draggedSlot.draggedIndex];
                inventory.Inventory[draggedSlot.draggedIndex] = temp;

                // 선택 인덱스 업데이트 (원하면)
                inventory.InventorySelect = slotIndex;

                // UI 갱신
                inventory.RefreshUI();
            }
        }
    }
}