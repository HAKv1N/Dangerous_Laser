using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] public GameObject escapeMenu;

    private CharacterController characterController;
    private PlayerStats playerStats;
    private Transform cameraTransform;
    [HideInInspector] public float rotationX;
    private Vector3 velocityDirection;
    private bool inGround;
    private bool isMoving;
    private bool isRunning;
    [HideInInspector] public float startFOV = 60;
    private UseGun useGun;
    [HideInInspector] public bool _canMove;
    private Settings settings;
    private float _currentSpeed;
    private Vector3 cameraRotation;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        playerStats = GetComponent<PlayerStats>();
        useGun = GetComponent<UseGun>();
        settings = GetComponentInChildren<Settings>(includeInactive: true);

        GetComponent<SaveManager>().Load();

        playerStats._currentHP = playerStats._maxHP;
        playerStats._currentStamina = playerStats._maxStamina;
        _canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Camera playerCamera = cameraTransform.GetComponent<Camera>();
        playerCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = settings.activePostProcessing;
    }

    private void Update()
    {
        UpdateEscMenu();
        Restart();

        if (!_canMove) return;

        Move();
        FirstPerson();
        Velocity();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        isMoving = h != 0 || v != 0;
        isRunning = isMoving && Input.GetKey(KeyCode.LeftShift) && playerStats._currentStamina > 0;

        Vector3 moveDirection = transform.forward * v + transform.right * h;

        Camera playerCamera = cameraTransform.GetComponent<Camera>();

        if (isRunning)
        {
            playerStats._currentStamina -= playerStats._staminaPerSecond * 2 * Time.deltaTime;
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV + 10, 5 * Time.deltaTime);
            _currentSpeed = playerStats._speed * 2;
            cameraRotation.z = Mathf.Lerp(cameraRotation.z, -h * 2, 10 * Time.deltaTime);
        }

        else if (isMoving)
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV + 2, 5 * Time.deltaTime);
            playerStats._currentStamina += playerStats._staminaPerSecond * 1.2f * Time.deltaTime;
            _currentSpeed = playerStats._speed;
            cameraRotation.z = Mathf.Lerp(cameraRotation.z, -h * 5, 10 * Time.deltaTime);
        }

        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV, 5 * Time.deltaTime);
            playerStats._currentStamina += playerStats._staminaPerSecond * 1.2f * Time.deltaTime;
            _currentSpeed = 0;
            cameraRotation.z = Mathf.Lerp(cameraRotation.z, 0, 10 * Time.deltaTime);
        }

        cameraRotation.z = Mathf.Clamp(cameraRotation.z, -5, 5);
        cameraTransform.localRotation = Quaternion.Euler(cameraRotation);

        characterController.Move(moveDirection.normalized * _currentSpeed * Time.deltaTime);
    }

    private void FirstPerson()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerStats._sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * playerStats._sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80, 80);

        transform.Rotate(Vector3.up * mouseX);

        cameraRotation.x = rotationX;
    }

    private void Velocity()
    {
        inGround = Physics.CheckSphere(checkGround.position, playerStats._radiusCheckGround, ~playerMask);

        Ray ray = new Ray(transform.localPosition, transform.up);

        if (Physics.Raycast(ray, 1, ~playerMask))
        {
            velocityDirection.y = -2f;
        }

        if (inGround && velocityDirection.y < 0)
        {
            velocityDirection.y = -2f;
        }

        else
        {
            velocityDirection.y -= playerStats._gravity * Time.deltaTime;
        }

        Jump();

        characterController.Move(velocityDirection * Time.deltaTime);
    }

    private void Jump()
    {
        if (inGround && Input.GetKeyDown(KeyCode.Space))
        {
            velocityDirection.y = Mathf.Sqrt(playerStats._jumpPower * 2f * playerStats._gravity);
        }
    }

    private void UpdateEscMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeMenu.SetActive(!escapeMenu.activeSelf);
            escapeMenu.GetComponentInChildren<Settings>().enabled = true;

            if (escapeMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            Cursor.visible = escapeMenu.activeSelf;

            useGun._canReload = !escapeMenu.activeSelf;
            useGun._canShoot = !escapeMenu.activeSelf;
            _canMove = !escapeMenu.activeSelf;
        }
    }

    private void Restart()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}