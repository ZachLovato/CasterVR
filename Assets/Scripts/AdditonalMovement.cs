using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.XR.Interaction.Toolkit;

public class AdditonalMovement : MonoBehaviour
{
	[Header("Action Input")]
	[SerializeField] private InputActionProperty UserJump;
	[SerializeField] private InputActionProperty UserCrouch;
	[SerializeField] private InputActionProperty UserSprint;

	[Space][SerializeField] private CharacterController characterController;

    [Space]
    [Header("Speed")]
    [SerializeField, Min(2)] private float maxSprintSpeed;
    [SerializeField, Min(1)] private float slowDownScale;
    [SerializeField, Min(1)] private float walkSpeed;
	private bool isSprinting = false;

	[Space]
	[Header("Jump")]
	[SerializeField] private float jumpHeight;
	[SerializeField] private float jumpScale;
	[SerializeField] private float fallScale;
	private Vector3 jumpVelocity;
	private bool isJumping = false;

	[Space]
	[Header("Crouch")]
	[SerializeField] private GameObject offset;
	[SerializeField, Range(0, 1)] private float crouchHeight;

	[Space]
    [SerializeField] private ActionBasedContinuousMoveProvider ConMoverAB;

    [HideInInspector] public bool isMoveBuffActive = false;

	// Start is called before the first frame update
	void Start()
    {
		UserSprint.action.started += startSprint;
		UserJump.action.started += startJump;

	}

	

	private void FixedUpdate()
	{
		//TODO --> change the read value to properly change the state of Crouching

		Vector2 c = UserCrouch.action.ReadValue<Vector2>();
		if (c.y < -.69 )
		{
			Vector3 temp = Vector3.zero;
			temp.y = -crouchHeight;
			offset.transform.localPosition = temp;
		}
		else
		{
			if (offset.transform.localPosition != Vector3.zero) offset.transform.localPosition = Vector3.zero;
		}

		characterJump();

		changeMovementSpeed();
	}



	#region  Movement
	private void changeMovementSpeed()
    {
		float temp = ConMoverAB.moveSpeed;

		if (isSprinting)
        {
            if (ConMoverAB.moveSpeed < maxSprintSpeed)
            {
				temp += Time.deltaTime;

				temp = capMovementSpeed(temp);
			}
        }
        else
        {
			if (ConMoverAB.moveSpeed > walkSpeed)
			{
				temp -= Time.deltaTime * slowDownScale;

				temp = capMovementSpeed(temp);
			}
		}

		ConMoverAB.moveSpeed = temp;
	}

    private float capMovementSpeed(float temp)
    {
        if (!isMoveBuffActive)
        {
			return Mathf.Clamp(temp, walkSpeed, maxSprintSpeed);
		}
		return temp;
    }

	// this section of the code was take from the channel Learn ICT Now, Jumping with Character Controller
	private void checkJump()
	{
		if (characterController.velocity.y == 0)
		{
			isJumping = true;
		}
	}

	private void characterJump()
	{
		if (characterController.isGrounded)
		{
			jumpVelocity.y = 0;
		}

		if (isJumping && characterController.isGrounded)
		{
			jumpVelocity.y += MathF.Sqrt(jumpHeight * -1 * -jumpScale);
			isJumping = false;
		}

		jumpVelocity.y += -fallScale * Time.deltaTime;
		characterController.Move(jumpVelocity);
	}


	#endregion

	#region  Button Started Functions
	private void startSprint(InputAction.CallbackContext ctx)
	{
		if (ctx.started) isSprinting = !isSprinting;
	}

	private void startJump(InputAction.CallbackContext ctx)
	{
		if (ctx.started) checkJump();
		else if (ctx.performed) isJumping = false;
		else if (ctx.canceled) isJumping = false;
	}

	#endregion
}
