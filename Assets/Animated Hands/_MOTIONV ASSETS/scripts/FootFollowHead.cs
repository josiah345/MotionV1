using UnityEngine;

public class FootFollowHead : MonoBehaviour
{
    public Transform head;
    public Transform origin;
    public Vector3 offset;
    public MotionVSerialReceiver receiver;
    public float moveSpeed = 1.5f;
    public float stopDistance = 0.3f;
    public float maxRayDistance = 20f;
    public LayerMask walkableMask = ~0;
    public float moveHoldTime = 0.4f;

    CharacterController controller;
    float moveUntil = 0f;

    void Awake()
    {
        controller = origin.GetComponent<CharacterController>();
    }

    void OnEnable()  { if (receiver) receiver.OnFootState += HandleFoot; }
    void OnDisable() { if (receiver) receiver.OnFootState -= HandleFoot; }

    void HandleFoot(int state)
    {
        if (state == 1)
            moveUntil = Time.time + moveHoldTime;
    }

    void Update()
    {
        if (Time.time > moveUntil) return;

        Vector3 target;
        if (Physics.Raycast(head.position, head.forward, out RaycastHit hit, maxRayDistance, walkableMask))
            target = hit.point;
        else
            target = head.position + head.forward * maxRayDistance;

        Vector3 toTarget = target - origin.position;
        toTarget.y = 0f;

        if (toTarget.magnitude <= stopDistance) return;

        Vector3 move = toTarget.normalized * moveSpeed * Time.deltaTime;
        move.y = -0.5f * Time.deltaTime;
        controller.Move(move);
    }

    void LateUpdate()
    {
        Vector3 pos = head.position + offset;
        pos.y = origin.position.y;
        transform.position = pos;
    }
}