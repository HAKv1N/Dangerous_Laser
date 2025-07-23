using UnityEngine;

public class GunInfo : MonoBehaviour
{
    public RecoilSettings recoilSettings;

    [Space]

    public int _maxAmmo;
    public int _currentAmmo;
    public bool _isAutomatically;
    public float _fireRate;
    public int _range;
    public int _damage;
    public float _reloadRate;
    public ParticleSystem _gunEffects;
    public LineRenderer _gunLine;
    public AudioClip _soundShoot;
    public AudioClip _soundReload;
    public AudioSource _audioSource;
    public Transform _muzzle;
    public Animator _gunAnimator;
    public Sprite _gunIcon;

    [Space]

    public GameObject _gunPrefab;
}

[System.Serializable]
public class RecoilSettings
{
    public float _verticalRecoil;
    public float _horizontalRecoil;
}