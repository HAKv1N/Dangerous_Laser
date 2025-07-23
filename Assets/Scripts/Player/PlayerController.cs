using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Value")]
    [SerializeField] private float _speed;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _gravity;
    [SerializeField] private float _radiusCheckGround;
    [SerializeField] private float _jumpPower;

    [Header("Objects")]
    [SerializeField] private Transform checkGround;
    [SerializeField] private LayerMask playerMask;

    private CharacterController characterController;
    private Transform cameraTransform;
    [HideInInspector] public float rotationX;
    private Vector3 velocityDirection;
    private bool inGround;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();

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

        characterController.Move(moveDirection.normalized * _speed * Time.deltaTime);
    }

    private void FirstPerson()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -80, 80);

        characterController.transform.Rotate(0, mouseX, 0);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void Velocity()
    {
        inGround = Physics.CheckSphere(checkGround.position, _radiusCheckGround, ~playerMask);

        if (inGround && velocityDirection.y < 0)
        {
            velocityDirection.y = -0.2f;
        }

        else
        {
            velocityDirection.y -= _gravity * Time.deltaTime;
        }

        Jump();

        characterController.Move(velocityDirection * Time.deltaTime);
    }

    private void Jump()
    {
        if (inGround && Input.GetKeyDown(KeyCode.Space))
        {
            velocityDirection.y = Mathf.Sqrt(_jumpPower * 2f * _gravity);
        }
    }
}