using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour
{
    [SerializeField]
    protected new Collider2D collider;

    public Rigidbody2D rb2D { get; protected set; }

    [SerializeField]
    protected float walkSpeed = 10;

    [SerializeField]
    protected float jumpSpeed = 10;
    
    public bool IsGrounded
    {
        get
        {
            return collider.IsTouchingLayers(CharacterManager.GroundMask);
        }
    }

    public bool Follow(Transform target, float minDistance, float speedMulitplier, out float velDirection)
    {
        Vector2 direction = target.position - transform.position;
        float distance = direction.magnitude;
        velDirection = 0;

        if (distance > minDistance)
        {

            float factor = distance - minDistance;
            factor /= CharacterManager.AccelerationFactor;
            factor = Mathf.Clamp(factor, 0, 1);

            Vector2 velocity = rb2D.velocity;
            velocity.x = (direction.normalized * walkSpeed * factor).x;
            velocity.x *= speedMulitplier;
            rb2D.velocity = velocity;
            LookAhead(velocity.x);

            velDirection = velocity.x;

            return true;
        }
        else
            return false;
    }

    protected void LookAhead(float forward)
    {
        RaycastHit2D hit = Physics2D.Raycast
            (
                transform.position,
                Vector2.right * forward,
                CharacterManager.DistanceToJump,
                CharacterManager.GroundMask
            );

        if (hit.collider)
        {
            Jump();
        }
    }

    protected virtual void Jump()
    {
        if (!IsGrounded)
            return;

        rb2D.AddForce(Vector2.up * jumpSpeed);
    }


}
