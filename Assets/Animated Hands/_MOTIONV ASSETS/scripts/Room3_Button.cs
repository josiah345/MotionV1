using UnityEngine;

public class Room3_Button : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("Name of this button (e.g., 'Pink', 'Blue'). Must match SequenceManager list.")]
    public string buttonID;
    public Room3_SequenceManager sequenceManager;

    [Header("Visuals & Audio")]
    public Transform buttonTop;
    public float pressDepth = 0.15f;
    public float moveSpeed = 15f;
    public AudioClip clickSound;

    [Header("Materials")]
    public Material defaultMaterial;
    public Material pressedMaterial;

    [Header("Settings")]
    public string activatorTag = "Open";

    private Vector3 restPos;
    private Renderer topRenderer;
    private bool locked;

    public bool isLockedDown
    {
        get => locked;
        set
        {
            locked = value;
            topRenderer.sharedMaterial = locked ? pressedMaterial : defaultMaterial;
        }
    }

    private void Awake()
    {
        restPos = buttonTop.localPosition;
        topRenderer = buttonTop.GetComponent<Renderer>();
        isLockedDown = false;
    }

    private void Update()
    {
        Vector3 targetPos = locked ? restPos + Vector3.down * pressDepth : restPos;
        buttonTop.localPosition = Vector3.Lerp(buttonTop.localPosition, targetPos, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(activatorTag) || locked) return;

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }

        sequenceManager.ButtonPressed(this);
    }

    public void ResetButton() => isLockedDown = false;
}