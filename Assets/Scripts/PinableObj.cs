using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PinableObj : XRGrabInteractable

{
    public Transform board;
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            board = other.transform;
        }
    
    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Board"))
        {
            board = null;
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (board != null)
        {
        
            rb.isKinematic = true;
        }
       
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        rb.isKinematic = false;
        base.OnSelectEntering(args);
       
    }
}

