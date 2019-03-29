using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public static ICharacter Lead;

    public static List<ICharacter> ControlledCharacters
    { get; private set; } = new List<ICharacter>();
    public static float MinimumDistanceToCarry { get => instance.minimumDistanceTocarry; }
    public static float DistanceFromLead { get => instance.distanceFromLead; }
    public static float DistanceBtwOthers { get => instance.distanceBtwOthers; }
    public static float AccelerationFactor { get => instance.accelerationFactor;}
    public static float DistanceFromLeadToDie { get => instance.distanceFromLeadToDie; }
    public static LayerMask GroundMask { get => instance.groundMask; }
    public static float DragOnGrounded { get => instance.dragOnGrounded;}
    public static float MaxVerticalSpeed { get => instance.maxVerticalSpeed;}
    public static float DistanceToJump { get => instance.distanceToJump; }
    public static bool FollowLead { get => instance.followLead; set => instance.followLead = value; }

    [SerializeField] private float minimumDistanceTocarry = 2;
    [SerializeField] private float distanceFromLead = 2;
    [SerializeField] private float distanceBtwOthers = 0.5f;
    [SerializeField] private float accelerationFactor = 0.5f;
    [SerializeField] private float dragOnGrounded = 10;
    [SerializeField] private float distanceFromLeadToDie = 20;
    [SerializeField] private float maxVerticalSpeed = 20;
    [SerializeField] private float distanceToJump = 1;
    [SerializeField] private bool followLead = true;
    [SerializeField] private LayerMask groundMask = -1;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Debug.Log("Multiple character managers");
    }

    private static bool calledPass = false;
    public static void PassLead(int amount = 1, bool ignoreCalled = false)
    {
        if (calledPass && !ignoreCalled)
            return;

        calledPass = true;

        int charsCount = ControlledCharacters.Count;
        if (Lead.gameObject == null)
            return;

        if (charsCount == 0)
        {
            GameManager.LoseGame();
            Lead = null;
            return;
        }

        int atualIndex = ControlledCharacters.IndexOf(Lead);
        int next = atualIndex + amount;

        if (next >= charsCount)
            next = 0;
        else if (next < 0)
            next = charsCount -1;
        
        ICharacter newLead = ControlledCharacters[next];
        if(newLead.gameObject == null)
        {
            ControlledCharacters.Remove(newLead);
            PassLead(ignoreCalled: true);
            return;
        }

        Lead = newLead;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            EvolveFamily();

        if (Input.GetKeyDown(KeyCode.E))
            PassLead(1);
        else if (Input.GetKeyDown(KeyCode.Q))
            PassLead(-1);

        if (Input.GetKeyDown(KeyCode.F))
            FollowLead = !FollowLead;
    }

    private void LateUpdate()
    {
        calledPass = false;
    }

    public static void OnKillCharacter(ICharacter character)
    {
        ControlledCharacters.Remove(character);

        if (Lead == character)
        {
            PassLead(ignoreCalled: true);
            if (Lead == character)
            {
                Lead = null;
            }
        }

        if(ControlledCharacters.Count == 0)
        {
            Lead = null;
            GameManager.LoseGame();
        }
    }

    public void EvolveFamily()
    {
        foreach (var character in ControlledCharacters.ToList())
        {
            EvolveCharacter(character);
        }
    }

    private void EvolveCharacter(ICharacter character)
    {
        int age = (int) character.CharAge;
        age++;
        if (age > (int)Character.Age.Elder)
        {
            character.Die();
        }
        else
        {
            character.Evolve((Character.Age) age);
        }
    }
}
