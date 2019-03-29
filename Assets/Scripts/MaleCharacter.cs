using System;
using UnityEngine;

public class MaleCharacter : Character
{
    private void Start()
    {
        gender = Gender.Male;
    }

    protected override void PerformAction()
    {
        if (CarriedGrabbable != null)
            Throw();
        else
            GrabCloserGrabbable();
    }

    ////MOVED TO AI AND CHARACTER
    //protected override void FollowLead(ICharacter lead)
    //{
    //    float speedMulitplier = 1;
    //    if (!IsGrounded)
    //    {
    //        if (WasThrown)
    //            return;
    //        speedMulitplier = 0.5f;
    //    }
    //    else
    //        WasThrown = false;

    //    int positionInLine = CharacterManager.ControlledCharacters.FindIndex(x => x == this);
    //    float distMin = (CharacterManager.DistanceBtwOthers * positionInLine) +
    //        CharacterManager.DistanceFromLead;

    //    Vector2 direction = lead.transform.position - transform.position;
    //    float distance = direction.magnitude;


    //    if (distance > distMin)
    //    {

    //        float factor = distance - distMin;
    //        factor /= CharacterManager.AccelerationFactor;
    //        factor = Mathf.Clamp(factor, 0, 1);

    //        Vector2 velocity = rb2D.velocity;
    //        velocity.x = (direction.normalized * walkSpeed * factor).x;
    //        velocity.x *= speedMulitplier;
    //        rb2D.velocity = velocity;

    //        SetLookDirection(velocity.x);

    //        LookAhead(velocity.x);
    //    }
    //    else
    //        anim.SetBool("Running", false);
    //}

    //protected void LookAhead(float forward)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast
    //        (
    //            transform.position,
    //            Vector2.right * forward, 
    //            CharacterManager.DistanceToJump,
    //            CharacterManager.GroundMask
    //        );

    //    if(hit.collider)
    //    {
    //        Jump();
    //    }
    //}

    private void SetLookDirection(float direction)
    {
        //set looking direction
        if (direction > 0)
            IsFacingRight = true;
        else if (direction < 0)
            IsFacingRight = false;

        //set animation flip
        spriteRenderer.flipX = !IsFacingRight;

        //set animation
        if (Mathf.Abs(direction) > 0.1f)
            anim.SetBool("Running", true);
        else
            anim.SetBool("Running", false);
    }

}
