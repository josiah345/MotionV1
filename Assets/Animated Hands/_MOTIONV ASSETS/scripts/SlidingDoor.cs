using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Parts")]
    [SerializeField] private Transform doorL;
    [SerializeField] private Transform doorR;

    [Header("Open Positions (Z)")]
    [SerializeField] private float leftOpenZ = 2.5f;
    [SerializeField] private float rightOpenZ = -0.7f;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float openTime = 10f;

    [Header("Events")]
    public UnityEvent onDoorClosed;

    private float leftClosedZ;
    private float rightClosedZ;
    private bool isMoving;

    private void Start()
    {
        leftClosedZ = doorL.localPosition.z;
        rightClosedZ = doorR.localPosition.z;
    }

    public void OpenDoor()
    {
        if (!isMoving) StartCoroutine(OpenAndClose());
    }

    private IEnumerator OpenAndClose()
    {
        isMoving = true;

        yield return MoveDoors(leftOpenZ, rightOpenZ);
        yield return new WaitForSeconds(openTime);
        yield return MoveDoors(leftClosedZ, rightClosedZ);

        onDoorClosed.Invoke();
        isMoving = false;
    }

    private IEnumerator MoveDoors(float leftZ, float rightZ)
    {
        while (!Mathf.Approximately(doorL.localPosition.z, leftZ) ||
               !Mathf.Approximately(doorR.localPosition.z, rightZ))
        {
            float step = moveSpeed * Time.deltaTime;
            MoveZ(doorL, leftZ, step);
            MoveZ(doorR, rightZ, step);
            yield return null;
        }
    }

    private static void MoveZ(Transform door, float targetZ, float step)
    {
        Vector3 pos = door.localPosition;
        pos.z = Mathf.MoveTowards(pos.z, targetZ, step);
        door.localPosition = pos;
    }
}