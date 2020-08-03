/*
* Jacob Buri
* .cs
* Assignment 6 - Factory Method
* Controlls Unit movement, Animator, and assigning targets
*/
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private Animator anim;
    public NavMeshAgent navAgent;
    LookAt lookAt;

    public bool isGrounded;
    public bool shouldMove = false;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        lookAt = gameObject.GetComponent<LookAt>();
        groundCheck = gameObject.transform;
        groundMask = LayerMask.GetMask("Ground");


        navAgent.radius = 1f;
        navAgent.height = 1f;
        navAgent.speed = 3f;
        navAgent.angularSpeed = 300;
        navAgent.acceleration = 8f;
        navAgent.stoppingDistance =.5f;
        // Don’t update position automatically
        navAgent.updatePosition = false;
    }

    void Update()
    {
        Vector3 worldDeltaPosition = navAgent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        //ApplyExtraTurnRotation();

        bool shouldMove = velocity.magnitude > 0.5f && 
            navAgent.remainingDistance > navAgent.stoppingDistance - 0.4f;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Update animation parameters
        // update the animator parameters
        anim.SetFloat("Forward", velocity.y);
        anim.SetFloat("Turn", velocity.x/10);
        anim.SetBool("OnGround", isGrounded);
        anim.SetBool("ShouldMove", shouldMove);

        float runCycle =
            Mathf.Repeat(
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.2f, 1);
        float jumpLeg = (runCycle < 0.5f ? 1 : -1) * velocity.y;
        if (isGrounded)
        {
            anim.SetFloat("JumpLeg", jumpLeg);
        }



        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (isGrounded && velocity.magnitude > 0)
        {
            anim.speed = 1.1f;
        }
        else
        {
            // don't use that while airborne
            anim.speed = 1;
        }

        /*
        if (lookAt)
            lookAt.lookAtTargetPosition = navAgent.steeringTarget + transform.forward;
        
        
        // Pull agent towards character
        if (worldDeltaPosition.magnitude > navAgent.radius)
            navAgent.nextPosition =  transform.position + 0.9f * worldDeltaPosition;
            */
        // Pull character towards agent
        if (worldDeltaPosition.magnitude > navAgent.radius)
        {
            // Pull agent towards character
            navAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;

            // Pull character towards agent
            //transform.position = navAgent.nextPosition - 0.9f * worldDeltaPosition;
        }


        
    }

    
    void OnAnimatorMove()
    {
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>();
        }
        else
        {
            // Update position based on animation movement using navigation surface height
            Vector3 position = anim.rootPosition;
            position.y = navAgent.nextPosition.y;
            transform.position = position;
        }
            
    }
    /*
    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(180, 360, velocity.x);
        transform.Rotate(0, velocity.y * turnSpeed * Time.deltaTime, 0);
    }

    
    private void Update()
    {
        if (currentTarget != null)
        {
            navAgent.destination = currentTarget.position;
        }
    }
    public void MoveUnit(Vector3 dest)
    {
        currentTarget = null;
        navAgent.destination = dest;
    }
    
    */


}