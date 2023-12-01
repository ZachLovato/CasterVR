using UnityEngine;
using UnityEngine.InputSystem;

public class animateHandOnInput : MonoBehaviour
{
    [SerializeField] private InputActionProperty pinchAnimationAction;
    [SerializeField] private InputActionProperty gripAnimationAction;
    [SerializeField] private Animator handAnimator;


    public enum STATES
    {
		DEFAULT,
		CASTING,
        UI
    }
    public STATES state = STATES.DEFAULT;  

    // Update is called once per frame
    void Update()
    {
        states();
	}

    private void states()
    {
        switch (state)
        {
            case STATES.DEFAULT:
                setPointFalse();
				float triggerValue = pinchAnimationAction.action.ReadValue<float>();
				handAnimator.SetFloat("Trigger", triggerValue);

				float gripValue = gripAnimationAction.action.ReadValue<float>();
				handAnimator.SetFloat("Grip", gripValue);
				break;
            case STATES.CASTING:
				handAnimator.SetFloat("Trigger", 1);
				handAnimator.SetFloat("Grip", 0);
				break;
            case STATES.UI:
                handAnimator.SetBool("isUIPoint", true);
                break;
        }
    }

    public void setPointFalse()
    {
        handAnimator.SetBool("isUIPoint", false);
	}
}
