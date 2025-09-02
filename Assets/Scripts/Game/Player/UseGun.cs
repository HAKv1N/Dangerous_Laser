using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UseGun : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform _playerHand;
    [SerializeField] private GameObject damageText;
    [SerializeField] private Transform damageDisplay;

    [HideInInspector] public GunInfo gunInfo;
    private float _nextFireTime;
    private Transform cameraTransform;
    [HideInInspector] public bool _canShoot = true;
    [HideInInspector] public bool _canReload = true;
    [HideInInspector] public bool _canTakeItem = true;
    private PlayerController playerController;
    private InventoryUI inventoryUI;
    private PlayerInventory playerInventory;

    private void Start()
    {
        cameraTransform = FindFirstObjectByType<Camera>().GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();
        inventoryUI = FindFirstObjectByType<InventoryUI>();
        playerInventory = GetComponent<PlayerInventory>();
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

        if (_canShoot && _nextFireTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !gunInfo._isAutomatically)
            {
                Shoot();
                _nextFireTime = gunInfo._fireRate;
            }

            else if (Input.GetKey(KeyCode.Mouse0) && gunInfo._isAutomatically)
            {
                Shoot();
                _nextFireTime = gunInfo._fireRate;
            }
        }

        _nextFireTime -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (gunInfo._currentAmmo <= 0 || !_canShoot)
        {
            gunInfo._audioSource.clip = gunInfo._soundNoBullets;
            gunInfo._audioSource.Play();

            return;
        }

        gunInfo._gunLine.enabled = true;
        gunInfo._gunLine.SetPosition(0, gunInfo._muzzle.position);

        Ray shootRay = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit shootHit;

        if (Physics.Raycast(shootRay, out shootHit, gunInfo._range))
        {
            bool isHeadShot = shootHit.collider.CompareTag("Head");

            EnemyInfo enemyInfo = shootHit.collider.GetComponentInParent<EnemyInfo>();

            if (enemyInfo != null)
            {
                float damage = gunInfo._damage;

                if (isHeadShot)
                {
                    damage *= gunInfo._headshotMulti;
                }

                enemyInfo._currentHP -= damage;

                GameObject newText = Instantiate(damageText, Vector3.zero, Quaternion.identity, damageDisplay);
                newText.GetComponent<Text>().text = "-" + damage + " HP";

                if (enemyInfo._currentHP <= 0)
                {
                    Destroy(enemyInfo.gameObject);
                    GetComponent<GameManager>()._destroyedEnemies++;
                }
            }
        }

        StartCoroutine(Recoil());

        gunInfo._gunLine.SetPosition(1, shootRay.origin + shootRay.direction * gunInfo._range);
        gunInfo._audioSource.clip = gunInfo._soundShoot;
        gunInfo._audioSource.Play();
        gunInfo._shootEffects.Play();
        gunInfo._gunAnimator.SetBool("Shoot", true);

        StartCoroutine(DisableGunLine(gunInfo._lineRate));

        gunInfo._currentAmmo--;

        inventoryUI.UpdateSlotUI(gunInfo._gunIcon, gunInfo._currentAmmo, playerInventory._currentSlotIndex);
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
        if (_canReload && gunInfo._currentAmmo < gunInfo._maxAmmo && playerInventory.currentItem != null)
        {
            gunInfo._audioSource.clip = gunInfo._soundReload;
            gunInfo._audioSource.Play();
            gunInfo._gunAnimator.SetBool("Reload", true);
            gunInfo._reloadEffects.Play();
            _canShoot = false;
            _canReload = false;
            _canTakeItem = false;

            yield return new WaitForSeconds(_reloadRate);

            _canShoot = !GetComponent<PlayerController>().escapeMenu.activeSelf;
            _canReload = true;
            _canTakeItem = true;
            gunInfo._currentAmmo = gunInfo._maxAmmo;
            gunInfo._gunAnimator.SetBool("Reload", false);
            inventoryUI.UpdateSlotUI(gunInfo._gunIcon, gunInfo._currentAmmo, playerInventory._currentSlotIndex);
        }
    }

    private IEnumerator Recoil()
    {
        float timer = 0;
        float currentRotationX = playerController.rotationX;
        float targetRotationX = currentRotationX - gunInfo.recoilSettings._verticalRecoil;
        
        float horizontalRecoil = Random.Range(-gunInfo.recoilSettings._horizontalRecoil, gunInfo.recoilSettings._horizontalRecoil);
        float currentRotationY = playerController.transform.localEulerAngles.y;
        float targetRotationY = currentRotationY + horizontalRecoil;

        while (timer < 0.05f)
        {
            timer += Time.deltaTime;
            float t = timer / 0.05f;
            
            playerController.rotationX = Mathf.Lerp(currentRotationX, targetRotationX, t);
            
            float newRotationY = Mathf.Lerp(currentRotationY, targetRotationY, t);
            Vector3 currentRotation = playerController.transform.localEulerAngles;
            playerController.transform.localEulerAngles = new Vector3(
                currentRotation.x, 
                newRotationY, 
                currentRotation.z
            );
            
            yield return null;
        }
    }
}