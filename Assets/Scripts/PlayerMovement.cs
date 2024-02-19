using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5.0f;
    public float sprintSpeed = 10.0f; // Speed while sprinting
    public float jumpForce = 7.0f;
    public float mouseSensitivity = 100.0f; // Sensitivity of mouse movement

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform; // Assuming the main camera is the child camera for the player
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    void Update()
    {
        Move();
        MouseLook();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Move()
    {
        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

        float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed;
        float moveVertical = Input.GetAxis("Vertical") * currentSpeed;

        Vector3 movement = transform.right * moveHorizontal + transform.forward * moveVertical;
        rb.MovePosition(transform.position + movement * Time.deltaTime);
    }

    void MouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
}
