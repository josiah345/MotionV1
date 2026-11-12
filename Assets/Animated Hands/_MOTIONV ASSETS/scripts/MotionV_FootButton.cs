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
    private Vector3 pressedPos;
    private Renderer topRenderer;
    private AudioSource audioSource;
    private int objectsOnButton = 0;

    void Start()
    {
        if (buttonTop != null)
        {
            restPos = buttonTop.localPosition;
            pressedPos = restPos - new Vector3(0, pressDepth, 0);
            topRenderer = buttonTop.GetComponent<Renderer>();
        }

        // Auto-add an audio source if a sound is assigned
        if (clickSound != null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f; // 3D sound
            }
        }

        if (defaultMaterial != null && topRenderer != null)
            topRenderer.material = defaultMaterial;
    }

    void Update()
    {
        if (buttonTop == null) return;

        // Smoothly move the button down or up
        Vector3 targetPos = (objectsOnButton > 0) ? pressedPos : restPos;
        buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object touching the button has the correct tag (e.g., "Open")
        if (other.CompareTag(activatorTag))
        {
            objectsOnButton++;

            // Only trigger if this is the FIRST thing touching it
            if (objectsOnButton == 1)
            {
                if (pressedMaterial != null && topRenderer != null)
                    topRenderer.material = pressedMaterial;

                if (audioSource != null && clickSound != null)
                    audioSource.PlayOneShot(clickSound);

                // 2. Fire the event! This is what opens the specific door.
                onButtonPressed.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(activatorTag))
        {
            objectsOnButton = Mathf.Max(0, objectsOnButton - 1);

            // Only release when ALL objects have left the button
            if (objectsOnButton == 0)
            {
                if (defaultMaterial != null && topRenderer != null)
                    topRenderer.material = defaultMaterial;

                onButtonReleased.Invoke();
            }
        }
    }
}