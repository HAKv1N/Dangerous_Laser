using UnityEngine;

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

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();
        playerStats = GetComponent<PlayerStats>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        Vector3 moveDirection = transform.forward * v + transform.right * h;

        characterController.Move(moveDirection.normalized * playerStats._speed * Time.deltaTime);
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