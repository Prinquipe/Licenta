using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	public float m_JumpForce;
	public float m_DoubleJumpForce;
	public float m_DashSpeed;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] public LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] public Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] public Transform m_CeilingCheck;                          // A position marking where to check for ceiling

	const float k_GroundedRadius = .5f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector2 m_Velocity = Vector2.zero;
	private Vector2 savedVelocity;
	private float savedGravity;
	private int jumps = 2;
	public const int maxJumps = 2;
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		savedGravity = m_Rigidbody2D.gravityScale;
	}

	private void Update()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		if (m_Rigidbody2D.velocity.y < 0)
		{
			CheckCollision(colliders);
		}
	}

	private void CheckCollision(Collider2D[] col)
	{
		bool isGrounded = false;
		for (int i = 0; i < col.Length; i++)
		{
			if (col[i].gameObject != gameObject)
			{
				isGrounded = true;
				if (isGrounded == !m_Grounded)
				{
					OnLandEvent.Invoke();
					m_Grounded = isGrounded;
					jumps = maxJumps;
					if (col[i].gameObject.CompareTag("Platform"))
					{
						transform.parent = col[i].gameObject.transform;
					}
					break;
				}
			}
		}
	}

	public void Move(float move, bool dash, bool jump,bool doubleJump)
	{
		Vector2 targetVelocity;
		if (dash)
		{

			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
				if (move > 0 || m_FacingRight)
				{
					targetVelocity = new Vector2(m_DashSpeed * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);
					m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity,.02f);
				}
				else if (move < 0 || !m_FacingRight)
				{
					targetVelocity = new Vector2(-m_DashSpeed * Time.fixedDeltaTime, m_Rigidbody2D.velocity.y);
					m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, .02f);
				}
				if (move > 0 && !m_FacingRight)
				{
					Flip();
				}
				else if (move > 0 && !m_FacingRight)
				{
					Flip();
				}
		}
		else
        {
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// Move the character by finding the target velocity
			targetVelocity = new Vector2(move, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector2.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			transform.parent = null;
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
		}
		else if(!m_Grounded && doubleJump	 && jumps>0)
        {
			transform.parent = null;
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_DoubleJumpForce), ForceMode2D.Impulse);
			jumps--;
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
