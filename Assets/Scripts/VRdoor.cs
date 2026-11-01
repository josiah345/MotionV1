using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; 
using UnityEngine.XR.Interaction.Toolkit.Interactors;
public class VRdoor : XRSimpleInteractable
{
    public Transform hinge;

    public Transform hand;

    public float doorAngle;
    public float startHandAngle;
    public float startDoorAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hand == null) return;

        float handMoved = Mathf.DeltaAngle(HandAngle(), startHandAngle);
        hinge.localRotation = Quaternion.Euler(0, startDoorAngle - handMoved, 0);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);
        hand = interactor.interactorObject.transform;

        startHandAngle = HandAngle();
        startDoorAngle = doorAngle; 
    }

    protected override void OnSelectExited(SelectExitEventArgs interactor)
    {
        base.OnSelectExited(interactor);
        hand = null;
    }

    float HandAngle()
    { 
       Vector3 handDir = hand.position - hinge.position;
       return Mathf.Atan2(handDir.x, handDir.z) * Mathf.Rad2Deg;

    }
    float CurrentAngle() => Mathf.Atan2(hinge.forward.x, hinge.forward.z) * Mathf.Rad2Deg;
}
