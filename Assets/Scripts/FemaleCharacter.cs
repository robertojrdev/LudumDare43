using System;
using UnityEngine;

public class FemaleCharacter : Character
{
    [SerializeField] private GameObject babyPrefab;
    private bool isPregnant = false;

    private void Start()
    {
        gender = Gender.Female;
    }

    protected override void PerformAction()
    {
        if (CarriedGrabbable != null)
            Throw();
        else
            GrabCloserGrabbable();
    }

    public override void Evolve(Age age)
    {
        if(CharAge == Age.Young && age == Age.Adult)
        {
            if (!isPregnant)
            {
                StartPregnancy();
                return;
            }
            else
            {
                HaveABaby();
                this.age = age;
                SetAnimLayerWeight(age);
            }
        }
        else
        {
            this.age = age;
            SetAnimLayerWeight(age);
        }
    }

    private void HaveABaby()
    {
        anim.SetLayerWeight(4, 0);
        Instantiate(babyPrefab, transform.position, Quaternion.identity);
    }

    private void StartPregnancy()
    {
        isPregnant = true;

        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
        anim.SetLayerWeight(3, 0);
        anim.SetLayerWeight(4, 1);
    }
}
