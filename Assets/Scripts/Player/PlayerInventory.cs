using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private Transform inventoryUI;

    [Header("Value")]
    [SerializeField] private float _rangeCheckItem;
    [SerializeField] private int _maxItemsOnInventory;

    private Transform cameraTransform;
    private List<GameObject> items = new List<GameObject>();
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private GameObject currentItem;

    private void Start()
    {
        cameraTransform = FindFirstObjectByType<Camera>().GetComponent<Transform>();

        InitializeSlots();
    }

    private void Update()
    {
        CheckItem();
        ChooseItemOnInventory();
    }

    private void CheckItem()
    {
        Ray rayCheckItem = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit itemHit;

        if (Physics.Raycast(rayCheckItem, out itemHit, _rangeCheckItem))
        {
            if (itemHit.collider.CompareTag("Item") && Input.GetKeyDown(KeyCode.E) && items.Count < _maxItemsOnInventory)
            {
                AddItemToInventory(itemHit.collider.transform);
            }
        }
    }

    private void AddItemToInventory(Transform item)
    {
        if (items.Count > _maxItemsOnInventory) return;

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
        for (int i = 0; i < _maxItemsOnInventory; i++)
        {
            InventorySlot inventorySlot = inventoryUI.GetChild(i).GetComponent<InventorySlot>();
            inventorySlots.Add(inventorySlot);
        }
    }
}