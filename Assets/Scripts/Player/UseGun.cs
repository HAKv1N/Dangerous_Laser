using System.Collections;
using UnityEngine;

public class UseGun : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform _playerHand;

    [HideInInspector] public GunInfo gunInfo;
    private float _nextFireTime;
    private Transform cameraTransform;
    private bool _canShoot = true;
    private bool _canReload = true;
    private PlayerController playerController;

    private void Start()
    {
        cameraTransform = FindFirstObjectByType<Camera>().GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckGun();
        Reload();
    }

    private void CheckGun()
    {
        if (_playerHand.childCount > 0)
        {
            gunInfo = _playerHand.GetComponentInChildren<GunInfo>();
        }

        else return;

        if (_canShoot)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !gunInfo._isAutomatically)
            {
                if (Time.time >= _nextFireTime)
                {
                    Shoot();
                    _nextFireTime = Time.time + gunInfo._fireRate;
                }
            }

            else if (Input.GetKey(KeyCode.Mouse0) && gunInfo._isAutomatically)
            {
                if (Time.time >= _nextFireTime)
                {
                    Shoot();
                    _nextFireTime = Time.time + gunInfo._fireRate;
                }
            }
        }
    }

    private void Shoot()
    {
        if (gunInfo._currentAmmo <= 0 || !_canShoot) return;

        gunInfo._gunLine.enabled = true;
        gunInfo._gunLine.SetPosition(0, gunInfo._muzzle.position);

        Ray shootRay = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit shootHit;

        if (Physics.Raycast(shootRay, out shootHit, gunInfo._range))
        {
            EnemyInfo enemyInfo = shootHit.collider.GetComponent<EnemyInfo>();

            if (enemyInfo != null)
            {
                enemyInfo._currentHP -= gunInfo._damage;

                if (enemyInfo._currentHP <= 0)
                {
                    Destroy(enemyInfo.gameObject);
                }
            }
        }

        Recoil();

        gunInfo._gunLine.SetPosition(1, shootRay.origin + shootRay.direction * gunInfo._range);
        gunInfo._audioSource.clip = gunInfo._soundShoot;
        gunInfo._audioSource.Play();
        gunInfo._gunEffects.Play();
        gunInfo._gunAnimator.SetBool("Shoot", true);
        StartCoroutine(DisableGunLine(0.07f));

        gunInfo._currentAmmo--;

        if (gunInfo._currentAmmo <= 0)
        {
            StartCoroutine(StartReload(gunInfo._reloadRate));

            return;
        }
    }

    IEnumerator DisableGunLine(float time)
    {
        yield return new WaitForSeconds(time);

        gunInfo._gunLine.enabled = false;
        gunInfo._gunAnimator.SetBool("Shoot", false);
    }

    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(StartReload(gunInfo._reloadRate));
        }
    }

    IEnumerator StartReload(float _reloadRate)
    {
        if (_canReload && gunInfo._currentAmmo < gunInfo._maxAmmo)
        {
            gunInfo._audioSource.clip = gunInfo._soundReload;
            gunInfo._audioSource.Play();
            gunInfo._gunAnimator.SetBool("Reload", true);
            _canShoot = false;
            _canReload = false;

            yield return new WaitForSeconds(_reloadRate);

            _canShoot = true;
            _canReload = true;
            gunInfo._currentAmmo = gunInfo._maxAmmo;
            gunInfo._gunAnimator.SetBool("Reload", false);
        }
    }

    private void Recoil()
    {
        playerController.rotationX -= gunInfo.recoilSettings._verticalRecoil;

        float horizontalRecoil = Random.Range(-gunInfo.recoilSettings._horizontalRecoil, gunInfo.recoilSettings._horizontalRecoil);
        playerController.transform.Rotate(0, horizontalRecoil, 0);
    }
}