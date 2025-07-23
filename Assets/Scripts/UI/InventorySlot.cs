using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Text bulletsText;

    public void UpdateSlotUI(Sprite newIcon, int bullets)
    {
        icon.sprite = newIcon;
        bulletsText.text = bullets.ToString();
    }
}