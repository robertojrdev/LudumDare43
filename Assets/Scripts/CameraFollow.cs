using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smooth = 0.1f;
    public Vector2 offset;

    void FixedUpdate()
    {
        if (CharacterManager.Lead == null)
            return;

        Vector3 nextPos = CharacterManager.Lead.transform.position;
        nextPos.z = -10;
        nextPos += (Vector3)offset;

        if (nextPos.x > GameManager.MaxXCamera)
        {
            nextPos.x = GameManager.MaxXCamera;
        }

        transform.position = Vector3.Lerp(transform.position, nextPos, smooth);
    }
}
