using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            slots.Add(transform.GetChild(i).GetComponent<InventorySlot>());
        }
    }

    public void UpdateSlotUI(Sprite newIcon, int bullets, int slotIndex)
    {
        slots[slotIndex].icon.sprite = newIcon;
        slots[slotIndex].bulletsText.text = bullets.ToString();
    }
}