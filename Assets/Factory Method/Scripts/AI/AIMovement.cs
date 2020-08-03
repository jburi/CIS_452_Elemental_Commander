using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
	public float speed = 12f;
	public float gravity = -9.81f;
	public float jumpHeight = 3f;

	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	Animator m_Animator;
	Vector3 velocity;
	bool isGrounded;

	private void Start()
	{
		m_Animator = GetComponentInChildren<Animator>();
		groundCheck = gameObject.transform;
	}

	void Update()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		//controller.Move(move * speed * Time.deltaTime);
		UpdateAnimator(move);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}

		velocity.y += gravity * Time.deltaTime;

		//controller.Move(velocity * Time.deltaTime);
	}

	void UpdateAnimator(Vector3 move)
	{
		// update the animator parameters
		m_Animator.SetFloat("Forward", Input.GetAxis("Vertical"), 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", Input.GetAxis("Mouse X"), 0.1f, Time.deltaTime);
		//m_Animator.SetBool("Crouch", m_Crouching);
		m_Animator.SetBool("OnGround", isGrounded);
		if (!isGrounded)
		{
			m_Animator.SetFloat("Jump", Mathf.Sqrt(jumpHeight * -2f * gravity));
		}

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1);
		float jumpLeg = (runCycle < 0.5f ? 1 : -1) * Input.GetAxis("Vertical");
		if (isGrounded)
		{
			m_Animator.SetFloat("JumpLeg", jumpLeg);
		}

		// the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
		// which affects the movement speed because of the root motion.
		if (isGrounded && move.magnitude > 0)
		{
			m_Animator.speed = 1;
		}
		else
		{
			// don't use that while airborne
			m_Animator.speed = 1;
		}
	}
}
