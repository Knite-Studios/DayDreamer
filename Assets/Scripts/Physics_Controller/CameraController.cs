using UnityEngine;

/// <summary>
/// Controls the camera movement and rotation based on the player's position and input.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector3 positionOffset;

    [SerializeField]
    private float distance = 10.0f;
    [SerializeField]
    private float rotateSpeed = 5.0f;
    [SerializeField]
    private float smoothness = 0.25f;
    [SerializeField]
    private float minAngle = -45.0f;
    [SerializeField]
    private float maxAngle = -10.0f;

    private Camera cam;
    private float currentX = 0.0f;
    private float currentY = 0.0f;

    /// <summary>
    /// Initialization of camera and cursor settings.
    /// </summary>
    void Start()
    {
        LockCursor();
        cam = Camera.main;
    }

    /// <summary>
    /// Handles user input for camera movement and rotation.
    /// </summary>
    void Update()
    {
        CheckForQuitCommand();
        UpdateCameraRotation();
    }

    /// <summary>
    /// Updates the camera's position and rotation in the physics loop.
    /// </summary>
    void FixedUpdate()
    {
        Vector3 direction = new Vector3(0, 1, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, -currentX, 0);
        cam.transform.position = Vector3.Lerp(cam.transform.position, player.position + rotation * direction, smoothness);
        cam.transform.LookAt(player.position + positionOffset);
    }

    /// <summary>
    /// Locks the cursor to the center of the screen and hides it.
    /// </summary>
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Checks for the 'Escape' key to quit the application.
    /// </summary>
    private void CheckForQuitCommand()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Updates the current X and Y rotation based on mouse input.
    /// </summary>
    private void UpdateCameraRotation()
    {
        currentX += Input.GetAxis("Mouse X") * rotateSpeed;
        currentY += Input.GetAxis("Mouse Y") * rotateSpeed;
        currentY = Mathf.Clamp(currentY, minAngle, maxAngle);
    }
}
