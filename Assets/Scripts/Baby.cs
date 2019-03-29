using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Baby : MonoBehaviour, ICharacter, IGrabbable
{
    public GameObject maleYoungPrefab;
    public GameObject femaleYoungPrefab;
    public GameObject deadbodyPrefab;
    public GrowTo growTo = GrowTo.Random;
    public enum GrowTo
    {
        Random, Male, Female
    }

    public bool WasThrown { get; set; }

    public Rigidbody2D rb2D { get; private set; }

    public ICarrier Carryer { get; private set; }
    
    public bool IsLead
    {
        get
        {
            return CharacterManager.Lead == this;
        }
    }

    public Character.Age CharAge { get; } = Character.Age.Baby;

    private void Awake()
    {
        GrabbableManager.Grabbables.Add(this);
        CharacterManager.ControlledCharacters.Add(this);

        gameObject.layer = 8;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public bool Grab(ICarrier carryer)
    {
        Carryer = carryer;
        rb2D.velocity = Vector2.zero;
        rb2D.simulated = false;
        transform.position = carryer.GrabPosition.position;
        transform.parent = carryer.GrabPosition;

        return true;
    }

    public void Release()
    {
        Carryer = null;
        rb2D.simulated = true;
        transform.parent = null;
    }

    public bool IsAvailableToGrab()
    {
        return Carryer == null;
    }

    public void Kill()
    {
        Destroy(gameObject);
        Instantiate(deadbodyPrefab, transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        CharacterManager.OnKillCharacter(this);
        GrabbableManager.Grabbables.Remove(this);
    }

    public void Evolve(Character.Age age)
    {
        if (Carryer != null)
            Carryer.Drop();

        GameObject newYoung = maleYoungPrefab;

        switch (growTo)
        {
            case GrowTo.Random:
                newYoung = Random.value > 0.5f ? maleYoungPrefab : femaleYoungPrefab;
                break;
            case GrowTo.Male:
                newYoung = maleYoungPrefab;
                break;
            case GrowTo.Female:
                newYoung = femaleYoungPrefab;
                break;
        }
        
        Instantiate(newYoung, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
        CharacterManager.OnKillCharacter(this);
    }

    public void Die()
    {
        Kill();
    }
}
