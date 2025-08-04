using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();

        for (int i = 0; i < 9; i++)
        {
            slots.Add(transform.GetChild(i).GetComponent<InventorySlot>());

            if (i > playerStats._maxItemsOnInventory - 1)
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void UpdateSlotUI(Sprite newIcon, int bullets, int slotIndex)
    {
        slots[slotIndex].icon.sprite = newIcon;
        slots[slotIndex].bulletsText.text = bullets.ToString();
    }
}