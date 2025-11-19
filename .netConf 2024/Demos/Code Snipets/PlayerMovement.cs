using UnityEngine;

// MonoBehaviour is a base class in Unity from which every script derives. It provides several important features and functionalities:

// Lifecycle Methods: MonoBehaviour includes methods like Start(), Update(), Awake(), FixedUpdate(), and LateUpdate(), 
// which are called at specific points in the script's lifecycle.

// Component System: Scripts that derive from MonoBehaviour can be attached to GameObjects as components, 
// allowing them to interact with other components and the GameObject itself.

// Coroutines: MonoBehaviour allows the use of coroutines, which are methods that can pause execution and resume at a later time, 
// useful for tasks like animations or timed events.

// Unity API Access: It provides access to Unity's API, enabling interaction with the game engine's features like physics, rendering, and input.
// In summary, MonoBehaviour is essential for creating scripts that interact with Unity's game engine and lifecycle.


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    private Rigidbody rb;
    private float rotationY = 0f;

    public Transform playerCamera; // Reference to the camera

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor for better FPS control
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * Time.deltaTime); // Moves the player character

        // Time.deltaTime is used to make movement frame rate independent. Here's why it's important:

        // Frame Rate Independence: Different devices and situations can cause the game to run at different frame rates.
        // Without Time.deltaTime, movement would be faster on higher frame rates and slower on lower frame rates.
        // Consistent Experience: By multiplying movement by Time.deltaTime, 
        //you ensure that the player moves the same distance per second, regardless of the frame rate.

        // In summary, Time.deltaTime normalizes movement to be consistent across different frame rates, 
        //providing a smooth and predictable experience.
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Limits vertical rotation

        // Rotate player body horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically
        playerCamera.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
    }
}


// A Quaternion in Unity is a complex number system used to represent rotations. 
// Here are some key points about Quaternion:

// Avoid Gimbal Lock: Unlike Euler angles, quaternions do not suffer from gimbal lock, 
// which is a situation where the axes of rotation can become aligned and cause a 
// loss of one degree of freedom.

// Smooth Interpolation: Quaternions allow for smooth interpolation between rotations, 
// which is useful for animations and camera movements.

// Compact Representation: A quaternion is represented by four components (x, y, z, w), 
// making it more compact than a 3x3 rotation matrix.

// Efficient Computation: Operations with quaternions are generally more 
// computationally efficient compared to matrices.

// In Unity, quaternions are used to handle rotations in 3D space, 
// providing a robust and efficient way to manage orientation and rotation.
