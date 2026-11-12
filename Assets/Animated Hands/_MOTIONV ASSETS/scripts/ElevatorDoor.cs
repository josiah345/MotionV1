using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    public Transform doorLeft;
    public Transform doorRight;

    public float leftOpenZ = 2.5f;
    public float rightOpenZ = -0.7f;
    public float speed = 3f;

    [Header("Delay Settings")]
    [Tooltip("How many seconds to wait after the player leaves before closing")]
    public float closeDelay = 5f;

    private float leftClosedZ;
    private float rightClosedZ;
    private bool isOpen;

    private void Start()
    {
        leftClosedZ = doorLeft.localPosition.z;
        rightClosedZ = doorRight.localPosition.z;
    }

    private void Update()
    {
        LerpZ(doorLeft, isOpen ? leftOpenZ : leftClosedZ);
        LerpZ(doorRight, isOpen ? rightOpenZ : rightClosedZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Open")) return;

        CancelInvoke(nameof(Close));
        isOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Open") || IsInvoking(nameof(Close))) return;

        Invoke(nameof(Close), closeDelay);
    }

    private void Close() => isOpen = false;

    private void LerpZ(Transform door, float targetZ)
    {
        Vector3 pos = door.localPosition;
        pos.z = Mathf.Lerp(pos.z, targetZ, Time.deltaTime * speed);
        door.localPosition = pos;
    }
}