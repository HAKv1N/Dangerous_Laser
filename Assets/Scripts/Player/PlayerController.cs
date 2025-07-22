using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Value")]
        [SerializeField] private float _speed;
        [SerializeField] private float _sensitivity;

        private CharacterController characterController;
        private Transform cameraTransform;
        private float rotationY;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            cameraTransform = FindAnyObjectByType<Camera>().GetComponent<Transform>();
        }

        private void Update()
        {
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

            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, -80, 80);

            characterController.transform.Rotate(0, mouseX, 0);
            cameraTransform.localRotation = Quaternion.Euler(rotationY, 0, 0);
        }
    }   
}