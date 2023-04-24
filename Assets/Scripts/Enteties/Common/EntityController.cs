using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityController : MonoBehaviour
{
    public AIBase AI;
    public Health health;

    public Rigidbody2D entityRigidbody;    
    public Animator animator;

    public EntitySoundController soundController;

    public float movementSpeed;

    public float jumpHeight
    {
        get { return _jumpHeight; }
        set
        {
            jumpForce = 10 * Mathf.Sqrt(0.2f * (value + 0.25f) * entityRigidbody.gravityScale);
            _jumpHeight = value + 0.25f;
        }
    }
    public float jumpForce { get; private set; }
    [SerializeField]
    protected float _jumpHeight;

    public bool isFacingRight = true;
    public bool isGrounded = false;
    public bool isMoving = false;

    [SerializeField]
    private int deathScore;

    public UnityEvent jumpEvent;

    public UnityEvent deathEvent;

    public void Jump()
    {
        jumpEvent.Invoke();
    }

    protected void Start()
    {
        jumpHeight = _jumpHeight;

        deathEvent.AddListener(() => LevelController.AddScore(deathScore));
    }

    public virtual void Flip()
    {
        isFacingRight = !isFacingRight;

        CustomMath.FlipTransform(transform);
        health.healthBar.Flip();
    }
}