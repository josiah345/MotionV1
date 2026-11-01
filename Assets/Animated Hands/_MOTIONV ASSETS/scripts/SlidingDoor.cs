using System.Collections;
using UnityEngine;

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

    private Vector3 leftClosedPos;
    private Vector3 rightClosedPos;
    private Vector3 leftOpenPos;
    private Vector3 rightOpenPos;

    private bool isMoving;

    void Start()
    {
        // Safety Check
        if (doorL == null || doorR == null)
        {
            Debug.LogError("Please assign Door Left and Door Right in the Inspector!");
            return;
        }

        leftClosedPos = doorL.localPosition;
        rightClosedPos = doorR.localPosition;

        // Keep original X and Y automatically, only change Z
        leftOpenPos = new Vector3(leftClosedPos.x, leftClosedPos.y, leftOpenZ);
        rightOpenPos = new Vector3(rightClosedPos.x, rightClosedPos.y, rightOpenZ);
    }

    public void OpenDoor()
    {
        if (!isMoving)
            StartCoroutine(OpenAndClose());
    }

    IEnumerator OpenAndClose()
    {
        isMoving = true;

        yield return MoveDoors(leftOpenPos, rightOpenPos);

        yield return new WaitForSeconds(openTime);

        yield return MoveDoors(leftClosedPos, rightClosedPos);

        isMoving = false;
    }

    IEnumerator MoveDoors(Vector3 leftTarget, Vector3 rightTarget)
    {
        while (Vector3.Distance(doorL.localPosition, leftTarget) > 0.01f ||
               Vector3.Distance(doorR.localPosition, rightTarget) > 0.01f)
        {
            doorL.localPosition = Vector3.MoveTowards(
                doorL.localPosition, leftTarget, moveSpeed * Time.deltaTime);

            doorR.localPosition = Vector3.MoveTowards(
                doorR.localPosition, rightTarget, moveSpeed * Time.deltaTime);

            yield return null;
        }

        // Snap exactly to the target to prevent tiny floating point errors
        doorL.localPosition = leftTarget;
        doorR.localPosition = rightTarget;
    }
}