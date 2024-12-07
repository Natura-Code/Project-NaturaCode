using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;

    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        // Cek apakah kita men-drop item ke slot yang valid
        GameObject dropTarget = eventData.pointerEnter;

        if (dropTarget != null && dropTarget.GetComponent<InventorySlot>() != null)
        {
            InventorySlot targetSlot = dropTarget.GetComponent<InventorySlot>();
            InventoryItem existingItem = targetSlot.GetComponentInChildren<InventoryItem>();

            // Jika slot kosong, pindahkan item
            if (existingItem == null)
            {
                transform.SetParent(targetSlot.transform);
                parentAfterDrag = targetSlot.transform;
            }
            // Jika ada item yang sama di slot dan masih ada ruang untuk stack
            else if (existingItem.item == this.item && existingItem.count < InventoryManager.instance.maxStackedItems)
            {
                int transferAmount = Mathf.Min(count, InventoryManager.instance.maxStackedItems - existingItem.count);
                existingItem.count += transferAmount;
                count -= transferAmount;
                existingItem.RefreshCount();
                RefreshCount();

                if (count <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                // Jika slot tidak valid, kembalikan ke posisi semula
                transform.SetParent(parentAfterDrag);
            }
        }
        else
        {
            // Kembalikan ke posisi semula jika bukan slot inventaris
            transform.SetParent(parentAfterDrag);
        }

        transform.localPosition = Vector3.zero; // Reset posisi lokal
    }

}
