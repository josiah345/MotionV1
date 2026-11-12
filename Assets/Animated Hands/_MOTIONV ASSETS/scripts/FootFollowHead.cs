using UnityEngine;

public class FootFollowHead : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Vector3 offset;

    public MotionVSerialReceiver receiver; // drag the receiver object here
    public float moveSpeed = 1.5f;         // metres per second

    bool moving = false;

    void OnEnable()  { if (receiver) receiver.OnFootState += HandleFoot; }
    void OnDisable() { if (receiver) receiver.OnFootState -= HandleFoot; }

    void HandleFoot(int state)
    {
        moving = (state == 1);
    }

    void Update()
    {
        if (!moving) return;

        Vector3 dir = head.forward;
        dir.y = 0f;
        dir.Normalize();

        origin.position += dir * moveSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        Vector3 pos = head.position + offset;
        pos.y = origin.position.y;
        transform.position = pos;
    }
}