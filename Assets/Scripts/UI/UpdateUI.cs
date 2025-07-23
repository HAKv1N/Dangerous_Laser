using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Text bulletsText;

    private UseGun useGun;

    private void Start()
    {
        useGun = FindFirstObjectByType<UseGun>();
    }

    public void UpdateUIVisual()
    {
        bulletsText.text = useGun.gunInfo._currentAmmo.ToString();
    }
}