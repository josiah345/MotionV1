using UnityEngine;

public class PushRigidbodies : MonoBehaviour
{
    public float pushPower = 3f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic) return;
        
        if (hit.moveDirection.y < -0.3f) return;

        Vector3 dir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        rb.AddForce(dir * pushPower, ForceMode.Impulse);
    }
}