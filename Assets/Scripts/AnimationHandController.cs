using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationHandController : MonoBehaviour
{

    public InputActionProperty triggerValue;
    public InputActionProperty grabValue;

    public Animator animator;



    // Update is called once per frame
    void Update()
    {
        float trigger = triggerValue.action.ReadValue<float>();
        float grip = grabValue.action.ReadValue<float>();

        animator.SetFloat("Trigger", trigger);
        animator.SetFloat("Grip", grip);
    }
}
