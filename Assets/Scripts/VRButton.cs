using UnityEngine;
using UnityEngine.Events;

public class VRbutton : MonoBehaviour
{

    public GameObject top;

    public UnityEvent onPress;
    public UnityEvent onrelease;

    public GameObject prefab;

    public bool isActive;

    public Material activeMat, notActiveMat;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        onPress.Invoke();

        top.transform.localPosition = Vector3.zero;
        
        isActive = !isActive;

        if (isActive)
        { 
            top.GetComponent<MeshRenderer>().material = activeMat;
        }
        else
        {
            top.GetComponent<MeshRenderer>().material = notActiveMat;
        }
    }

    private void OnTriggerExit(Collider other)
        {
        onrelease.Invoke();
        
        top.transform.localPosition = new Vector3(0, 0.493f, 0);
    }
}
