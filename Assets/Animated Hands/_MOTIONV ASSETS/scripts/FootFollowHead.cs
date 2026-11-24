using UnityEngine;

public class FootFollowHead : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Vector3 offset;

    public MotionVSerialReceiver receiver;
    public float moveSpeed = 1.5f;
    public float stopDistance = 0.3f;   // stop when this close to the target
    public float maxRayDistance = 20f;
    public LayerMask walkableMask = ~0; // layers the gaze ray can hit

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

        Vector3 target;
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, maxRayDistance, walkableMask))
            target = hit.point;
        else
            target = head.position + head.forward * maxRayDistance;

        Vector3 toTarget = target - origin.position;
        toTarget.y = 0f; // stay on the floor

        if (toTarget.magnitude <= stopDistance) return;

        origin.position += toTarget.normalized * moveSpeed * Time.deltaTime;
    }

    void LateUpdate()
    {
        Vector3 pos = head.position + offset;
        pos.y = origin.position.y;
        transform.position = pos;
    }
}