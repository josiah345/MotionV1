using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class VRdoor : MonoBehaviour
{
    [SerializeField] private Transform doorL, doorR;
    [SerializeField] private float leftOpenZ = 2.5f, rightOpenZ = -0.7f;
    [SerializeField] private float moveSpeed = 2f, openTime = 10f;
    public UnityEvent onDoorClosed;

    private float leftClosedZ, rightClosedZ, closeTimer;
    private bool isOpen, isMoving;

    private void Start()
    {
        leftClosedZ = doorL.localPosition.z;
        rightClosedZ = doorR.localPosition.z;
    }

    public void OpenDoor()
    {
        closeTimer = openTime;
        if (!isOpen && !isMoving) StartCoroutine(DoorCycle());
    }

    private IEnumerator DoorCycle()
    {
        isMoving = true;
        yield return Slide(leftOpenZ, rightOpenZ);
        isOpen = true;
        isMoving = false;

        while (closeTimer > 0f) { closeTimer -= Time.deltaTime; yield return null; }

        isMoving = true;
        isOpen = false;
        yield return Slide(leftClosedZ, rightClosedZ);
        isMoving = false;
        onDoorClosed.Invoke();
    }

    private IEnumerator Slide(float lz, float rz)
    {
        while (!Mathf.Approximately(doorL.localPosition.z, lz) ||
               !Mathf.Approximately(doorR.localPosition.z, rz))
        {
            float s = moveSpeed * Time.deltaTime;
            SetZ(doorL, lz, s);
            SetZ(doorR, rz, s);
            yield return null;
        }
    }

    private static void SetZ(Transform t, float z, float s)
    {
        var p = t.localPosition; p.z = Mathf.MoveTowards(p.z, z, s); t.localPosition = p;
    }
}