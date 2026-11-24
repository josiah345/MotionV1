using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public Transform target;         
    public Vector3 offset = new Vector3(0, 5, 0);  

    void LateUpdate()
    {
        transform.position = target.position + offset;
       
    }
}