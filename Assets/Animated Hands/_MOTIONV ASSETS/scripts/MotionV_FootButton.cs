using UnityEngine;
using UnityEngine.Events;

public class MotionV_FootButton : MonoBehaviour
{
    [Header("Visual References")]
    [Tooltip("The physical part of the button that moves down.")]
    public Transform buttonTop;

    [Tooltip("How far down the button goes when stepped on (in meters).")]
    public float pressDepth = 0.03f;

    [Tooltip("How fast the button moves down and up.")]
    public float moveSpeed = 15f;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material pressedMaterial;

    [Header("Settings")]
    [Tooltip("The tag of the object allowed to press this button (e.g., 'Open').")]
    public string activatorTag = "Open";

    [Header("Audio Feedback (Optional)")]
    public AudioClip clickSound;

    [Header("Events")]
    public UnityEvent onButtonPressed;
    public UnityEvent onButtonReleased;

    private Vector3 restPos;
    private Renderer topRenderer;
    private int objectsOnButton;

    private void Awake()
    {
        restPos = buttonTop.localPosition;
        topRenderer = buttonTop.GetComponent<Renderer>();
        topRenderer.sharedMaterial = defaultMaterial;
    }

    private void Update()
    {
        Vector3 targetPos = objectsOnButton > 0 ? restPos + Vector3.down * pressDepth : restPos;
        buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(activatorTag)) return;

        objectsOnButton++;
        if (objectsOnButton > 1) return;

        topRenderer.sharedMaterial = pressedMaterial;

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }

        onButtonPressed.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(activatorTag)) return;

        objectsOnButton = Mathf.Max(0, objectsOnButton - 1);
        if (objectsOnButton > 0) return;

        topRenderer.sharedMaterial = defaultMaterial;
        onButtonReleased.Invoke();
    }
}