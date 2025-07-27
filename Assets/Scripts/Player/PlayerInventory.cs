using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private LayerMask playerMask;

    private InventoryUI inventoryUI;
    private Transform cameraTransform;
    private PlayerStats playerStats;
    private UseGun useGun;
    private List<GameObject> items = new List<GameObject>();
    private GameObject currentItem;
    [HideInInspector] public int _currentSlotIndex = -1;

    private void Start()
    {
        cameraTransform = FindFirstObjectByType<Camera>().GetComponent<Transform>();
        playerStats = GetComponent<PlayerStats>();
        useGun = GetComponent<UseGun>();
        inventoryUI = FindFirstObjectByType<InventoryUI>();
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

            if (Physics.Raycast(rayCheckItem, out itemHit, playerStats._rangeCheckItem, ~playerMask))
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

        GunInfo gunInfo = item.GetComponent<GunInfo>();
        inventoryUI.UpdateSlotUI(gunInfo._gunIcon, gunInfo._currentAmmo, items.Count - 1);
    }

    private void ChooseItemOnInventory()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) && useGun._canTakeItem)
            {
                TakeItemToHand(i);
            }
        }
    }

    private void TakeItemToHand(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= items.Count) return;

        if (currentItem != null && _currentSlotIndex >= 0)
        {
            ReturnItemToInventory(_currentSlotIndex, currentItem.GetComponent<GunInfo>());
        }

        currentItem = Instantiate(items[slotIndex], playerHand);
        currentItem.SetActive(true);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
        currentItem.GetComponent<GunInfo>()._currentAmmo = items[slotIndex].GetComponent<GunInfo>()._currentAmmo;

        _currentSlotIndex = slotIndex;
    }

    private void ReturnItemToInventory(int slotIndex, GunInfo gunInfo)
    {
        items[slotIndex].GetComponent<GunInfo>()._currentAmmo = gunInfo._currentAmmo;
        Destroy(currentItem);
        currentItem = null;
    }
}