using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask playerMask;

    private CharacterController characterController;
    private PlayerStats playerStats;
    private Transform cameraTransform;
    [HideInInspector] public float rotationX;
    private Vector3 velocityDirection;
    private bool inGround;
    private bool isMoving;
    private bool isRunning;
    private float startFOV;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = GetComponentInChildren<Camera>().GetComponent<Transform>();
        playerStats = GetComponent<PlayerStats>();
        startFOV = cameraTransform.GetComponent<Camera>().fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerStats._currentHP = playerStats._maxHP;
        playerStats._currentStamina = playerStats._maxStamina;
    }

    private void Update()
    {
        Velocity();
        Move();
        FirstPerson();
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
            characterController.Move(moveDirection.normalized * playerStats._speed * 2 * Time.deltaTime);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV + 10, 5 * Time.deltaTime);
        }

        else if (isMoving)
        {
            characterController.Move(moveDirection.normalized * playerStats._speed * Time.deltaTime);
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV + 2, 5 * Time.deltaTime);
            playerStats._currentStamina += playerStats._staminaPerSecond * 1.2f * Time.deltaTime;
        }

        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, startFOV, 5 * Time.deltaTime);
            playerStats._currentStamina += playerStats._staminaPerSecond * 1.2f * Time.deltaTime;
        }
    }

    private void FirstPerson()
    {
        float mouseX = Input.GetAxis("Mouse X") * playerStats._sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * playerStats._sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80, 80);

        characterController.transform.Rotate(0, mouseX, 0);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void Velocity()
    {
        inGround = Physics.CheckSphere(checkGround.position, playerStats._radiusCheckGround, ~playerMask);

        if (inGround && velocityDirection.y < 0)
        {
            velocityDirection.y = -0.2f;
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
}