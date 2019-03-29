using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene4 : MonoBehaviour
{
    bool hasEntered = false;
    bool finished = false;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!hasEntered || finished)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Hide");
            finished = true;
        }
    }

    public void OnEnterCharacter()
    {
        hasEntered = true;
        anim.SetTrigger("Show");
    }
}
