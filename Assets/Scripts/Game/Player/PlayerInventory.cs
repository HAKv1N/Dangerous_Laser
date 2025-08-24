using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform playerHand;
    [SerializeField] private GameObject playerArms;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private LayerMask playerMask;

    private InventoryUI inventoryUI;
    private Transform cameraTransform;
    private PlayerStats playerStats;
    private UseGun useGun;
    private List<GameObject> items = new List<GameObject>();
    [HideInInspector] public GameObject currentItem;
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
        DropItem();

        playerArms.SetActive(!currentItem);
    }

    private void CheckItem()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray rayCheckItem = new Ray(cameraTransform.position, cameraTransform.forward);
            RaycastHit itemHit;

            if (Physics.Raycast(rayCheckItem, out itemHit, playerStats._rangeCheckItem, ~playerMask))
            {
                if (itemHit.collider.CompareTag("Item"))
                {
                    AddItemToInventory(itemHit.collider.transform);

                    return;
                }
            }
        }
    }

    private void AddItemToInventory(Transform item)
    {
        bool isEmpty = true;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                isEmpty = false;

                break;
            }
        }

        if (isEmpty && items.Count >= playerStats._maxItemsOnInventory) return;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item.gameObject;

                item.gameObject.SetActive(false);
                item.SetParent(inventoryTransform);
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;

                GunInfo gunInfoNew = item.GetComponent<GunInfo>();
                inventoryUI.UpdateSlotUI(gunInfoNew._gunIcon, gunInfoNew._currentAmmo, i);

                if (i == 0)
                {
                    TakeItemToHand(0);
                }

                return;
            }
        }

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
        if (slotIndex < 0 || slotIndex >= items.Count || items[slotIndex] == null) return;

        if (currentItem != null && _currentSlotIndex >= 0)
        {
            ReturnItemToInventory(_currentSlotIndex, currentItem.GetComponent<GunInfo>());
        }

        currentItem = Instantiate(items[slotIndex], playerHand);
        currentItem.SetActive(true);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.identity;
        currentItem.GetComponent<GunInfo>()._currentAmmo = items[slotIndex].GetComponent<GunInfo>()._currentAmmo;
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        currentItem.GetComponent<Collider>().enabled = false;

        _currentSlotIndex = slotIndex;
    }

    private void ReturnItemToInventory(int slotIndex, GunInfo gunInfo)
    {
        items[slotIndex].GetComponent<GunInfo>()._currentAmmo = gunInfo._currentAmmo;
        Destroy(currentItem);
        currentItem = null;
    }

    private void DropItem()
    {
        if (Input.GetKeyDown(KeyCode.Q) && currentItem != null && useGun._canReload && useGun._canShoot)
        {
            currentItem.transform.SetParent(null);
            currentItem.transform.localRotation = Quaternion.identity;

            Rigidbody itemRB = currentItem.GetComponent<Rigidbody>();
            itemRB.isKinematic = false;
            itemRB.AddForce(cameraTransform.forward * 3 + cameraTransform.up * 0.3f, ForceMode.Impulse);

            currentItem.GetComponent<Collider>().enabled = true;

            currentItem.GetComponent<GunInfo>()._audioSource.clip = null;

            items[_currentSlotIndex] = null;
            currentItem = null;

            inventoryUI.UpdateSlotUI(null, 0, _currentSlotIndex);
        }
    }
}