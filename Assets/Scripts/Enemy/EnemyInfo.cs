using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public int _maxHP;
    [HideInInspector] public int _currentHP;
    public bool _agressive;
    public float _rangeAttack;
    public float _damage;
    public float _fireRate;
    public float _maxAmmo;
    [HideInInspector] public float _currentAmmo;
    public float _reloadRate;

    private void Awake()
    {
        _currentHP = _maxHP;
        _currentAmmo = _maxAmmo;
    }
}