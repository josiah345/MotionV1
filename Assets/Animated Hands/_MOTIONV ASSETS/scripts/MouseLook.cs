using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Player object here")]
    public Transform playerBody;

    [Header("Settings")]
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    void Start()
    {
        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        // Read mouse movement (New Input System)
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Rotate the camera up and down (Pitch)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Stop the camera from flipping upside down
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player body left and right (Yaw)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}