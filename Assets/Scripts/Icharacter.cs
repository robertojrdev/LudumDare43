using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    Character.Age CharAge { get; }
    GameObject gameObject { get; }
    Transform transform { get; }
    string name { get; set; }

    void Kill();
    void Die();
    void Evolve(Character.Age age);
    bool IsLead { get; }
}
