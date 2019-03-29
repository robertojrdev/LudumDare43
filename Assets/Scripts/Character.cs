using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public abstract class Character : AI, ICharacter, IGrabbable, ICarrier
{
    public static bool CanMove { get; set; } = true;

    //VARIABLES
    protected Gender gender;

    [SerializeField]
    protected Age age = Age.Young;

    [SerializeField]
    protected float throwForce = 10;

    [SerializeField]
    protected bool forceLeadAtStart = false;

    [SerializeField]
    protected Transform carryPosition;

    [SerializeField]
    protected GameObject deadbodyPrefab;

    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    public IGrabbable CloserGrabbable { get; private set; }
    public IGrabbable CarriedGrabbable { get; private set; }
    public ICarrier Carryer { get; private set; }

    protected bool IsFacingRight { get; set; }
    public bool WasThrown { get; set; }
    
    public bool IsLead
    {
        get
        {
            return CharacterManager.Lead == this;
        }
    }

    public Gender CharGender
    {
        get
        {
            return gender;
        }

        private set
        {
            gender = value;
        }
    }
    public Age CharAge
    {
        get
        {
            return age;
        }

        private set
        {
            age = value;
        }
    }

    public Transform GrabPosition
    {
        get { return carryPosition; }
    }

    public enum Gender
    {
        Male, Female
    }
    public enum Age
    {
        Baby, Young, Adult, Elder
    }

    //METHODS
    protected virtual void Awake()
    {
        gameObject.layer = 8;

        CharacterManager.ControlledCharacters.Add(this);
        GrabbableManager.Grabbables.Add(this);

        rb2D = GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if(!collider)
        collider = GetComponent<CapsuleCollider2D>();

        if (CharacterManager.Lead == null || forceLeadAtStart)
            CharacterManager.Lead = this;

        SetAnimLayerWeight(age);
    }

    private void Update()
    {
        if (CharacterManager.Lead != null)
        {
            if (!IsLead)
            {
                //if far from lead it will die
                float distanceFromLead = Vector2.Distance(
                    base.transform.position, CharacterManager.Lead.transform.position);
                if (distanceFromLead > CharacterManager.DistanceFromLeadToDie)
                {
                    Kill();
                    return;
                }

                //follow (if not die)
                FollowLead(CharacterManager.Lead);
            }
            else
            {
                CloserGrabbable = GrabbableManager.GetCloserAvailableGrabbable(base.transform.position, this);
                GetInputs();
            }
        }

        if (IsGrounded && !IsLead)
            rb2D.drag = CharacterManager.DragOnGrounded;
        else
            rb2D.drag = 0;

        Tick(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //limit y velocity
        Vector2 velocity = rb2D.velocity;

        velocity.y = Mathf.Clamp(velocity.y,
            -CharacterManager.MaxVerticalSpeed,
            CharacterManager.MaxVerticalSpeed);

        rb2D.velocity = velocity;
    }

    protected virtual void Tick(float deltaTime) { }

    protected virtual void GetInputs()
    {
        if (!CanMove)
            return;

        Walk(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Action"))
            PerformAction();

        if (Input.GetButtonDown("Jump"))
            Jump();
    }

    protected virtual void Walk(float direction)
    {
        direction = Mathf.Clamp(direction, -1, 1);
        Vector2 velocity = rb2D.velocity;
        velocity.x = direction * walkSpeed;
        rb2D.velocity = velocity;

        //set looking direction
        if (direction > 0)
            IsFacingRight = true;
        else if (direction < 0)
            IsFacingRight = false;

        //set animation flip
        spriteRenderer.flipX = !IsFacingRight;

        //set animation
        if (velocity.x != 0)
            anim.SetBool("Running", true);
        else
            anim.SetBool("Running", false);
    }

    protected void FollowLead(ICharacter lead)
    {
        if (!CharacterManager.FollowLead)
        {
            anim.SetBool("Running", false);
            return;
        }

        float speedMulitplier = 1;
        if (!IsGrounded)
        {
            if (WasThrown)
                return;
            speedMulitplier = 0.5f;
        }
        else
            WasThrown = false;

        int positionInLine = CharacterManager.ControlledCharacters.FindIndex(x => x == this);
        float distMin = (CharacterManager.DistanceBtwOthers * positionInLine) +
            CharacterManager.DistanceFromLead;

        float direction;
        if (Follow(lead.transform, distMin, speedMulitplier, out direction))
        {
            anim.SetBool("Running", true);
            SetLookDirection(direction);
        }
        else
            anim.SetBool("Running", false);
    }

    protected abstract void PerformAction();

    /// <summary>
    /// carry the closer character
    /// </summary>
    public void GrabCloserGrabbable()
    {
        if (CloserGrabbable == null || Carryer != null)
            return;

        float distance = Vector2.Distance(CloserGrabbable.transform.position, base.transform.position);

        if(distance < CharacterManager.MinimumDistanceToCarry &&
            CloserGrabbable.Grab(this))
        {
            CarriedGrabbable = CloserGrabbable;
            anim.SetBool("Holding", true);
        }
    }

    public void GrabGrabbable(IGrabbable grabbable)
    {
        if (CloserGrabbable == null || Carryer != null)
            return;

        grabbable.Grab(this);
        anim.SetBool("Holding", true);
    }

    public bool IsAvailableToGrab()
    {
        if (CarriedGrabbable != null || Carryer != null)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Be carried
    /// </summary>
    /// <param name="carryer">carryer character</param>
    /// <returns></returns>
    public bool Grab(ICarrier carryer)
    {
        Carryer = carryer;
        rb2D.velocity = Vector2.zero;
        rb2D.simulated = false;
        base.transform.position = carryer.GrabPosition.position;
        base.transform.parent = carryer.GrabPosition;

        return true;
    }

    public void Drop()
    {
        if (CarriedGrabbable == null)
            return;

        CarriedGrabbable.Release();
        CarriedGrabbable = null;
        anim.SetBool("Holding", false);
    }

    public void Release()
    {
        Carryer = null;
        rb2D.simulated = true;
        base.transform.parent = null;
    }

    public void Throw()
    {
        if (CarriedGrabbable == null)
            return;

        //stop carry
        CarriedGrabbable.Release();
        //calculate force
        Vector2 force = new Vector2(0.5f, 0.5f) * throwForce;
        //calculate direction
        force.x *= IsFacingRight ? 1 : -1;
        //apply
        CarriedGrabbable.rb2D.AddForce(force);

        CarriedGrabbable.WasThrown = true;

        //release
        CarriedGrabbable = null;
    }

    public void Kill()
    {
        Instantiate(deadbodyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        GrabbableManager.Grabbables.Remove(this);
        CharacterManager.OnKillCharacter(this);
    }

    public virtual void Evolve(Age age)
    {
        CharAge = age;
        SetAnimLayerWeight(age);
    }

    protected virtual void SetAnimLayerWeight(Age age)
    {
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(3, 0);

        switch (age)
        {
            case Age.Baby:
                break;
            case Age.Young:
                anim.SetLayerWeight(3, 1);
                break;
            case Age.Adult:
                anim.SetLayerWeight(2, 1);
                break;
            case Age.Elder:
                anim.SetLayerWeight(1, 1);
                break;
            default:
                break;
        }
    }

    public void Die()
    {
        Release();
        Kill();
    }


    private void SetLookDirection(float direction)
    {
        //set looking direction
        if (direction > 0)
            IsFacingRight = true;
        else if (direction < 0)
            IsFacingRight = false;

        //set animation flip
        spriteRenderer.flipX = !IsFacingRight;
    }
}
