using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterTrigger : MonoBehaviour
{
    public UnityEvent onEnter;
    public UnityEvent onLeadEnter;
    public UnityEvent onOtherEnter;
    public UnityEvent onExit;
    public UnityEvent onLeadExit;
    public UnityEvent onOtherExit;

    private void Awake()
    {
        gameObject.layer = 9;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICharacter c = collision.GetComponent<ICharacter>();
        if (c == null)
            return;

        onEnter.Invoke();
        if (c.IsLead)
            onLeadEnter.Invoke();
        else
            onOtherEnter.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ICharacter c = collision.GetComponent<ICharacter>();
        if (c == null)
            return;

        onExit.Invoke();
        if (c.IsLead)
            onLeadExit.Invoke();
        else
            onOtherExit.Invoke();
    }
}
