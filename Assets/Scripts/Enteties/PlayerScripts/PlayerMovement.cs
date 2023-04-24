using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    static PlayerMovement()
    {
        anim_JumpTrigger = Animator.StringToHash("Jump");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
        anim_IsMoving = Animator.StringToHash("IsMoving");
    }

    private static int anim_JumpTrigger;
    private static int anim_IsGrounded;
    private static int anim_IsMoving;

    public static PlayerMovement _instance;

    [SerializeField] private Collider2D groundCollider;
    [SerializeField] private PlayerController playerController;

    [SerializeField]
    private bool isJumping = false;

    private float timeWhenGrounded;
    private float timeWhenJumped;

    [SerializeField]
    private float coyoteTime;
    [SerializeField]
    private float jumpBufferTime;

    int movementInput = 0;

    public int maxNumberOfJumps = 2;
    public int currentJumpsRemaning = 0;

    public bool FlipCheckIsOverridden = false;

    [SerializeField] [Range(0, 1)] private float frictionUnderKnockback;
    [Range(0,1)] public float movementFriction;
    
    [SerializeField] private float jumpReleaseDamping;

    private const float movementAcceleration = 40; //Constant for how long the acceleration will take, 1 / n seconds

    private bool jumpAnimationIsRunning = false;

    public bool isAffectedByKnockback = false;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            UnityEngine.Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }


    public void GetMovementInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAffectedByKnockback = false;
            playerController.animator.SetBool(anim_IsMoving, true);

            movementInput = (int)context.ReadValue<float>();
        }
        else if (context.canceled)
        {
            movementInput = 0;

            playerController.animator.SetBool(anim_IsMoving, false);
        }

        
    }

    public void FixedUpdate()
    {
        if(false == playerController.isGrounded)
        {
            CorrectForLedge();
        }

        if (false == FlipCheckIsOverridden && movementInput * playerController.transform.localScale.x < 0) //Checks if the player should flip direction
        {
            playerController.Flip();
        }

        if (movementInput != 0)
        {
            if (Mathf.Sign((playerController.movementSpeed * movementInput) - playerController.entityRigidbody.velocity.x) == movementInput) //Not moving att max speed
            {
                playerController.entityRigidbody.velocity += new Vector2(playerController.movementSpeed * movementInput * Time.deltaTime * movementAcceleration, 0);
            }
        }
        else
        {
            if(false == isAffectedByKnockback)
            {
                playerController.entityRigidbody.velocity = new Vector2(playerController.entityRigidbody.velocity.x * movementFriction, playerController.entityRigidbody.velocity.y);
            }
            else
            {
                if (true == playerController.isGrounded)
                {
                    playerController.entityRigidbody.velocity = new Vector2(playerController.entityRigidbody.velocity.x * frictionUnderKnockback, playerController.entityRigidbody.velocity.y);
                }
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GroundCheck.CheckForGrounded(collision))
        {
            EnteredGround();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (true == playerController.isGrounded)
        {
            LeftGround();
        }            
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GroundCheck.CheckForGrounded(collision))
        {
            if (false == playerController.isGrounded)
            {
                EnteredGround();
            }
            
        } else
        {
            if (true == playerController.isGrounded)
            {
                LeftGround();
            }
        }
    }


    public void GetJumpInput(InputAction.CallbackContext context)
    {
        if (context.started) //Button down
        {
            if (currentJumpsRemaning > 0) //Any jumps remaining to do the jump
            {
                if (playerController.isGrounded || Time.time - timeWhenGrounded <= coyoteTime) //Grounded or coyptee
                {
                    playerController.jumpEvent.Invoke();
                }
                else
                {
                    if (false == isJumping) //Has not jumped already but coyotee time is over, can not perform primary jump
                    {
                        currentJumpsRemaning--;
                    }

                    if(currentJumpsRemaning != 0)
                    {
                        playerController.jumpEvent.Invoke(); //Secondary jump
                    }
                    
                }
            }
            else
            {
                timeWhenJumped = Time.time;
            }
        }
        else if (context.canceled) //Button up
        {
            if (playerController.entityRigidbody.velocity.y > 0)
            {
                playerController.entityRigidbody.velocity = new Vector2(playerController.entityRigidbody.velocity.x, playerController.entityRigidbody.velocity.y * jumpReleaseDamping);
            }
        }
    }

    public void Jump()
    {
        playerController.animator.SetTrigger(anim_JumpTrigger);
        playerController.animator.SetBool(anim_IsGrounded, false);

        if (playerController.entityRigidbody.velocity.y > 0) //if already moving up (like in a double jump), increase velocity by jump force
        {
            playerController.entityRigidbody.velocity += new Vector2(0, playerController.jumpForce);
        }
        else
        {
            playerController.entityRigidbody.velocity = new Vector2(playerController.entityRigidbody.velocity.x, playerController.jumpForce); //if standing still or falling set the velocity to the jump force so to cancel a fall.
        }


        timeWhenJumped = 0;

        isJumping = true;

        currentJumpsRemaning--;

        jumpAnimationIsRunning = true;
    }

    public void Jump(float jumpForce)
    {
        playerController.animator.SetTrigger(anim_JumpTrigger);
        playerController.animator.SetBool(anim_IsGrounded, false);

        if (playerController.entityRigidbody.velocity.y > 0) //if already moving up (like in a double jump), increase velocity by jump force
        {
            playerController.entityRigidbody.velocity += new Vector2(0, jumpForce);
        }
        else
        {
            playerController.entityRigidbody.velocity = new Vector2(playerController.entityRigidbody.velocity.x, jumpForce); //if standing still or falling set the velocity to the jump force so to cancel a fall.
        }


        timeWhenJumped = 0;

        isJumping = true;

        currentJumpsRemaning--;

        jumpAnimationIsRunning = true;
    }



    private void EnteredGround()
    {
        jumpAnimationIsRunning = false;

        playerController.animator.SetBool(anim_IsGrounded, true);

        playerController.isGrounded = true;

        timeWhenGrounded = 0;

        isJumping = false;

        currentJumpsRemaning = maxNumberOfJumps;

        if (Time.time - timeWhenJumped < jumpBufferTime) //Jump buffer
        {
            playerController.jumpEvent.Invoke();
        }
    }

    private void LeftGround()
    {
        playerController.animator.SetBool(anim_IsGrounded, false);

        if (jumpAnimationIsRunning == false)
        {
            //playerController.animator.SetTrigger(anim_JumpTrigger);
            jumpAnimationIsRunning = true;
        }

        playerController.isGrounded = false;

        timeWhenGrounded = Time.time;
    }

    private void CorrectForLedge()
    {
        if (DetectLedge())
        {
            playerController.entityRigidbody.position += new Vector2(WallRaycastDistance * (playerController.isFacingRight ? 1 : -1), LedgeRaycastOffset);
            EnteredGround();
        }
    }

    private const float LedgeRaycastOffset = 0.2f;
    private const float WallRaycastDistance = 0.2f;
    private const float X_Offset = 0.52f / 2;
    private const float Y_Offset = -0.21f + -1.250993f / 2 + 0.04f;

    private bool DetectLedge()
    {
        int direction = playerController.isFacingRight ? 1 : -1;

        Vector2 raycastOrigin = (Vector2)playerController.playerTransform.position + new Vector2(X_Offset * direction, Y_Offset);

        //Debug.DrawRay(raycastOrigin, direction * WallRaycastDistance * Vector2.right, Color.yellow);
        //Debug.DrawRay(raycastOrigin + new Vector2(0, LedgeRaycastOffset), direction * WallRaycastDistance * Vector2.right, Color.yellow);

        if (Physics2D.Raycast(raycastOrigin, Vector2.right * direction, WallRaycastDistance, CommonLayerMasks.GroundCheckLayers))
        {
            raycastOrigin.y += LedgeRaycastOffset;

            if(false == Physics2D.Raycast(raycastOrigin, Vector2.right * direction, WallRaycastDistance, CommonLayerMasks.GroundCheckLayers))
            {
                return true;
            }
        }

        return false;
    }
}
