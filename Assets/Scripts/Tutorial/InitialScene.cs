using UnityEngine;

public class InitialScene : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            anim.SetBool("HitEnter", true);
        }
    }
}
