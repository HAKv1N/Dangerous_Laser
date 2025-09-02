using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public float _maxHP;
    [HideInInspector] public float _currentHP;
    public bool _agressive;
    public float _rangeAttack;
    public float _damage;
    public float _fireRate;
    public float _maxAmmo;
    [HideInInspector] public float _currentAmmo;
    public float _reloadRate;
    public ParticleSystem _shootEffects;
    public AudioClip _shootSound;
    public AudioClip _reloadSound;
    public AudioClip _missSound;
    public Slider slider;
    public TextMeshProUGUI bulletsText;

    private void Awake()
    {
        _currentHP = _maxHP;
        _currentAmmo = _maxAmmo;
    }

    private void Update()
    {
        slider.value = _currentHP;
        bulletsText.text = _currentAmmo.ToString();
    }
}