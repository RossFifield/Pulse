using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;
    public float strafeSpeedMultiplier = 1.5f;

    private CharacterController characterController;
    private float pitch = 0.0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Lock cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse look
        float yaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.Rotate(0, yaw, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Check for strafe
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKey(KeyCode.A))
            {
                x -= strafeSpeedMultiplier;
            }
            if (Input.GetKey(KeyCode.D))
            {
                x += strafeSpeedMultiplier;
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * movementSpeed * Time.deltaTime);
    }
}
