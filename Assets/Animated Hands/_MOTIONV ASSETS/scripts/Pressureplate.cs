using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("References")]
    public Transform plateTop;
    public SlidingDoor door;

    [Header("Plate Settings")]
    public float pressDistance = 0.05f;
    public Color inactiveColor = Color.yellow;
    public Color activeColor = Color.red;

    private Vector3 releasedPosition;
    private Vector3 pressedPosition;
    private Renderer plateRenderer;

    // Tracks how many PushBlocks are currently on the plate
    private int objectsOnPlate = 0;

    void Start()
    {
        if (plateTop != null)
        {
            releasedPosition = plateTop.localPosition;
            pressedPosition = releasedPosition + Vector3.down * pressDistance;
            plateRenderer = plateTop.GetComponent<Renderer>();
            SetColor(inactiveColor);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PushBlock"))
            return;

        objectsOnPlate++;

        // Only trigger the door if this is the FIRST block to touch it
        if (objectsOnPlate == 1)
        {
            if (plateTop != null) plateTop.localPosition = pressedPosition;
            SetColor(activeColor);

            if (door != null)
            {
                door.OpenDoor();
            }

            Debug.Log("Pressure Plate Activated");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("PushBlock"))
            return;

        objectsOnPlate--;

        // Only close the door if ALL blocks have left the plate
        if (objectsOnPlate <= 0)
        {
            objectsOnPlate = 0; // Safety catch
            if (plateTop != null) plateTop.localPosition = releasedPosition;
            SetColor(inactiveColor);

            Debug.Log("Pressure Plate Deactivated");
        }
    }

    void SetColor(Color color)
    {
        if (plateRenderer != null)
        {
            plateRenderer.material.color = color;
        }
    }
}