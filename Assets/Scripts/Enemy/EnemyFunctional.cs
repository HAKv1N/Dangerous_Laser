using System.Collections;
using UnityEngine;

public class EnemyFunctional : MonoBehaviour
{
    private PlayerController playerController;
    private EnemyInfo enemyInfo;
    private float _nextFireTime;
    private bool _canShoot = true;
    private bool _canReload = true;
    private Quaternion startRotation;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        enemyInfo = GetComponent<EnemyInfo>();

        startRotation = transform.localRotation;
    }

    private void Update()
    {
        CheckTarget(playerController.transform);
    }

    private void CheckTarget(Transform target)
    {
        Vector3 targetVector = target.position;
        float distanceToTarget = Vector3.Distance(transform.position, targetVector);

        if (distanceToTarget <= enemyInfo._rangeAttack)
        {
            Ray rayCheck = new Ray(transform.position, targetVector - transform.position);
            RaycastHit hit;

            if (Physics.Raycast(rayCheck, out hit, enemyInfo._rangeAttack))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Vector3 direction = hit.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

                    if (Time.time > _nextFireTime)
                    {
                        ShootEnemy();

                        _nextFireTime = Time.time + enemyInfo._fireRate;

                        return;
                    }
                }
            }
        }

        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 5 * Time.deltaTime);
        }
    }

    private void ShootEnemy()
    {
        if (!enemyInfo._agressive || !_canShoot || enemyInfo._currentAmmo <= 0) return;

        PlayerStats playerStats = playerController.GetComponent<PlayerStats>();

        playerStats.GetDamage(enemyInfo._damage);

        if (playerStats._currentHP <= 0)
        {
            Destroy(playerController.gameObject);
        }

        enemyInfo._currentAmmo--;

        if (enemyInfo._currentAmmo <= 0 && _canReload)
        {
            StartCoroutine(StartReloadEnemy(enemyInfo._reloadRate));
        }
    }

    IEnumerator StartReloadEnemy(float time)
    {
        _canReload = false;
        _canShoot = false;

        yield return new WaitForSeconds(time);

        _canReload = true;
        _canShoot = true;
        enemyInfo._currentAmmo = enemyInfo._maxAmmo;
    }
}