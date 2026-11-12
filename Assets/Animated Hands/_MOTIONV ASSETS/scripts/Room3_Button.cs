using UnityEngine;

public class Room3_Button : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("Name of this button (e.g., 'Pink', 'Blue'). Must match SequenceManager list.")]
    public string buttonID;
    public Room3_SequenceManager sequenceManager;

    [Header("Visuals & Audio")]
    public Transform buttonTop;
    public float pressDepth = 0.15f; // <--- INCREASED DEFAULT SO YOU CAN SEE IT
    public float moveSpeed = 15f;
    public AudioClip clickSound;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material pressedMaterial;

    [Header("Settings")]
    public string activatorTag = "Open";

    [HideInInspector] public bool isLockedDown = false;

    private Vector3 restPos;
    private Vector3 pressedPos;
    private AudioSource audioSource;
    private Renderer topRenderer;
    private bool wasLockedDown = false; // To track when to change color

    void Start()
    {
        if (buttonTop != null)
        {
            restPos = buttonTop.localPosition;
            pressedPos = restPos - new Vector3(0, pressDepth, 0);
            topRenderer = buttonTop.GetComponent<Renderer>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && clickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // Set the starting material
        if (defaultMaterial != null && topRenderer != null)
            topRenderer.material = defaultMaterial;
    }

    void Update()
    {
        if (buttonTop == null) return;

        // 1. Move the button down or up
        Vector3 targetPos = isLockedDown ? pressedPos : restPos;
        buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * moveSpeed);

        // 2. Change the material based on the state!
        if (isLockedDown != wasLockedDown)
        {
            wasLockedDown = isLockedDown;

            if (topRenderer != null)
            {
                if (isLockedDown && pressedMaterial != null)
                    topRenderer.material = pressedMaterial;
                else if (!isLockedDown && defaultMaterial != null)
                    topRenderer.material = defaultMaterial;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activatorTag) && !isLockedDown)
        {
            if (audioSource != null && clickSound != null)
                audioSource.PlayOneShot(clickSound);

            if (sequenceManager != null)
            {
                sequenceManager.ButtonPressed(this);
            }
        }
    }

    public void ResetButton()
    {
        isLockedDown = false;
    }
}