using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

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
    public PlayableDirector playableDirector;
    public CinemachineCamera startCamera;
    public CinemachineCamera continueCamera;

    private void Awake()
    {
        _currentHP = _maxHP;
        _currentStamina = _maxStamina;
    }

    public void GetDamage(float damage, GameObject Enemy)
    {
        _currentHP -= damage;

        if (_currentHP <= 0)
        {
            Animator animator = Enemy.GetComponent<Animator>();

            Dead(animator, Enemy.transform);
        }
    }

    private void Dead(Animator animator, Transform enemyHead)
    {
        GetComponent<PlayerController>()._canMove = false;

        TimelineAsset timelineAsset = playableDirector.playableAsset as TimelineAsset;

        CinemachineBrain brain = FindFirstObjectByType<Camera>().GetComponent<CinemachineBrain>();
        brain.enabled = true;

        continueCamera.transform.SetParent(enemyHead);
        continueCamera.transform.localPosition = Vector3.zero + Vector3.up * 0.7f + Vector3.forward;
        continueCamera.transform.localRotation = Quaternion.Euler(0, 180, 0);

        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track.name == "Enemy Anim")
            {
                playableDirector.SetGenericBinding(track, animator);
            }
        }

        playableDirector.Play();
        StartCoroutine(KillPlayer((float)playableDirector.duration));
    }

    IEnumerator KillPlayer(float duration)
    {
        yield return new WaitForSeconds(duration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }
}