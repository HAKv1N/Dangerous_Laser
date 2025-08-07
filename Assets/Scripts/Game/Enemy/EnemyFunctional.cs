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
    private Animator animator;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        enemyInfo = GetComponent<EnemyInfo>();
        animator = GetComponent<Animator>();

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
            animator.SetBool("Shoot", false);
        }
    }

    private void ShootEnemy()
    {
        if (!enemyInfo._agressive || !_canShoot || enemyInfo._currentAmmo <= 0) return;

        animator.SetBool("Shoot", true);

        PlayerStats playerStats = playerController.GetComponent<PlayerStats>();

        playerStats.GetDamage(enemyInfo._damage, gameObject);

        enemyInfo._currentAmmo--;

        enemyInfo._shootEffects.Play();

        AudioSource source = GetComponent<AudioSource>();
        source.clip = enemyInfo._shootSound;
        source.Play();

        if (enemyInfo._currentAmmo <= 0 && _canReload)
        {
            StartCoroutine(StartReloadEnemy(enemyInfo._reloadRate));
        }
    }

    IEnumerator StartReloadEnemy(float time)
    {
        _canReload = false;
        _canShoot = false;

        animator.SetBool("Shoot", false);
        animator.SetBool("Reload", true);

        AudioSource source = GetComponent<AudioSource>();
        source.clip = enemyInfo._reloadSound;
        source.Play();

        yield return new WaitForSeconds(time);

        _canReload = true;
        _canShoot = true;
        enemyInfo._currentAmmo = enemyInfo._maxAmmo;

        animator.SetBool("Reload", false);
    }
}