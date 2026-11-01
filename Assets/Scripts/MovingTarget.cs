using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    // how fast the target slides left and right
    public float speed = 2f;

    // how far it travels in each direction from its starting position
    public float range = 3f;

    private Vector3 startPos;
    private float dir = 1f; // 1 = right, -1 = left

    void Start()
    {
        // save starting position before any movement
        startPos = transform.position;
    }

    void Update()
    {
        // move in current direction — Time.deltaTime keeps it smooth
        transform.position += Vector3.right * speed * dir * Time.deltaTime;

        // flip direction when boundary is reached
        if (transform.position.x >= startPos.x + range) dir = -1f;
        if (transform.position.x <= startPos.x - range) dir = 1f;
    }
}