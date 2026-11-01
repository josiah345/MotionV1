using System.Collections;
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

    private Vector3 leftClosedPos, leftOpenPos;
    private Vector3 rightClosedPos, rightOpenPos;
    private bool isOpen = false;

    private Coroutine closeTimer;

    void Start()
    {
        leftClosedPos = doorLeft.localPosition;
        rightClosedPos = doorRight.localPosition;

        leftOpenPos = new Vector3(leftClosedPos.x, leftClosedPos.y, leftOpenZ);
        rightOpenPos = new Vector3(rightClosedPos.x, rightClosedPos.y, rightOpenZ);
    }

    void Update()
    {
        doorLeft.localPosition = Vector3.Lerp(doorLeft.localPosition, isOpen ? leftOpenPos : leftClosedPos, Time.deltaTime * speed);
        doorRight.localPosition = Vector3.Lerp(doorRight.localPosition, isOpen ? rightOpenPos : rightClosedPos, Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Open"))
        {
            if (closeTimer != null)
            {
                StopCoroutine(closeTimer);
                closeTimer = null;
            }

            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Open"))
        {
            if (closeTimer == null)
            {
                closeTimer = StartCoroutine(CloseAfterDelay());
            }
        }
    }

    IEnumerator CloseAfterDelay()
    {
        yield return new WaitForSeconds(closeDelay);

        isOpen = false;
        closeTimer = null;
    }
}