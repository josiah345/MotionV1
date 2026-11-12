using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Player object here")]
    public Transform playerBody;

    [Header("Settings")]
    public float mouseSensitivity = 100f;

    private float xRotation;

    private void Start() => Cursor.lockState = CursorLockMode.Locked;

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 look = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation - look.y, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * look.x);
    }
}