using UnityEngine;

public class PushObjects : MonoBehaviour
{
    public float pushPower = 2f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (rb == null)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        rb.AddForce(pushDir * pushPower, ForceMode.Impulse);
    }
}