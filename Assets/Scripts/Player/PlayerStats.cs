using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float _speed;
    public float _sensitivity;
    public float _gravity;
    public float _radiusCheckGround;
    public float _jumpPower;
    public float _rangeCheckItem;
    public int _maxItemsOnInventory;
    public float _maxHP;
    [HideInInspector] public float _currentHP;
    public float _maxStamina;
    [HideInInspector] public float _currentStamina;
    public float _staminaPerSecond;

    private void Awake()
    {
        _currentHP = _maxHP;
        _currentStamina = _maxStamina;
    }

    public void GetDamage(float damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        
    }

    private void Update()
    {
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }
}