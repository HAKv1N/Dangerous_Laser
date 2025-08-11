using UnityEngine;
using UnityEngine.UI;

public class InventoryStats : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Text killsText;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }

    private void Update()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        hpSlider.maxValue = playerStats._maxHP;
        staminaSlider.maxValue = playerStats._maxStamina;

        hpSlider.value = playerStats._currentHP;
        staminaSlider.value = playerStats._currentStamina;

        GameManager gameManager = GetComponentInParent<GameManager>();
        SpawnEnemy spawnEnemy = FindFirstObjectByType<SpawnEnemy>();

        killsText.text = gameManager._destroyedEnemies + "/" + spawnEnemy._maxEnemies;
    }
}