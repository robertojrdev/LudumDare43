using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillRegion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICharacter c = collision.GetComponent<ICharacter>();
        if (c == null)
            return;

        c.Kill();
    }
}
