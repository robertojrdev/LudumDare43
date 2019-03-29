using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    private static InGameUI instance;

    private Animator anim;

    private void Awake()
    {
        if(!instance)
            instance = this;

        anim = GetComponent<Animator>();
    }

    public static void FadeIn()
    {
        if (!instance)
            return;
        //reset
        instance.anim.ResetTrigger("Fade Out");
        instance.anim.ResetTrigger("Fade Out");

        instance.anim.SetTrigger("Fade In");
    }

    public static void FadeOut()
    {
        if (!instance)
            return;
        //reset
        instance.anim.ResetTrigger("Fade Out");
        instance.anim.ResetTrigger("Fade Out");

        instance.anim.SetTrigger("Fade Out");
    }
}
