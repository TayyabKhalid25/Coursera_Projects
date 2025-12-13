using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Movement speed in units per second")]
    public float moveSpeed = 5f;

    [Tooltip("Mouse sensitivity for looking around")]
    public float mouseSensitivity = 2f;

    public bool isLookingAtTarget = false;

    private float pitch = 0f;
    private float yaw = 0f;
    private bool cursorLocked = true;

    void Start()
    {
        LockCursor(true);
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    void Update()
    {
        // ESC always unlocks the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(false);
        }

        // Click to re-lock the cursor
        if (Input.GetMouseButtonDown(0) && !cursorLocked)
        {
            LockCursor(true);
        }

        // Rotate camera if cursor is locked and not looking at a target
        if (cursorLocked && !isLookingAtTarget)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        // WASD movement + Space/Ctrl
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f;
        if (Input.GetKey(KeyCode.Space)) moveY += 1f;
        if (Input.GetKey(KeyCode.LeftControl)) moveY -= 1f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ + Vector3.up * moveY;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Exit Game
        if (Input.GetKeyDown(KeyCode.P)) ExitGame();
    }

    void LockCursor(bool shouldLock)
    {
        cursorLocked = shouldLock;
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }


    void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in Editor
        #else
                Application.Quit(); // Quit the built application
        #endif
    }
}