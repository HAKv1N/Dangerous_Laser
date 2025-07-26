using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private Transform inventoryUI;

    private Transform cameraTransform;
    private PlayerStats playerStats;
    private List<GameObject> items = new List<GameObject>();
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private GameObject currentItem;

    private void Start()
    {
        cameraTransform = FindFirstObjectByType<Camera>().GetComponent<Transform>();
        playerStats = GetComponent<PlayerStats>();

        InitializeSlots();
    }

    private void Update()
    {
        CheckItem();
        ChooseItemOnInventory();
    }

    private void CheckItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray rayCheckItem = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit itemHit;

            if (Physics.Raycast(rayCheckItem, out itemHit, playerStats._rangeCheckItem))
            {
                if (itemHit.collider.CompareTag("Item") && items.Count < playerStats._maxItemsOnInventory)
                {
                    AddItemToInventory(itemHit.collider.transform);

                    return;
                }
            }
        }
    }

    private void AddItemToInventory(Transform item)
    {
        if (items.Count > playerStats._maxItemsOnInventory) return;

        items.Add(item.gameObject);
        item.gameObject.SetActive(false);
        item.SetParent(inventoryTransform);
        item.localPosition = Vector3.zero;
        item.localRotation = Quaternion.identity;

        if (items.Count == 1)
        {
            TakeItemToHand(0);
        }

        for (int i = 0; i < items.Count; i++)
        {
            GunInfo gunInfo = items[i].GetComponent<GunInfo>();

            inventorySlots[i].icon.sprite = gunInfo._gunIcon;

            inventorySlots[i].UpdateSlotUI(gunInfo._gunIcon, gunInfo._currentAmmo);
        }
    }

    private void ChooseItemOnInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TakeItemToHand(i);
            }
        }
    }

    private void TakeItemToHand(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count) return;

        if (currentItem != null)
        {
            Destroy(currentItem);
            currentItem = null;
        }

        currentItem = Instantiate(items[slotIndex], playerHand);
        currentItem.SetActive(true);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < playerStats._maxItemsOnInventory; i++)
        {
            InventorySlot inventorySlot = inventoryUI.GetChild(i).GetComponent<InventorySlot>();
            inventorySlots.Add(inventorySlot);
        }
    }
}