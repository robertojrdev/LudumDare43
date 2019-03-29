using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastScene : MonoBehaviour
{
    float timer = 0;

    private void Start()
    {
        timer = Time.timeSinceLevelLoad;
    }

    private void Update()
    {
        if (timer + 7 < Time.timeSinceLevelLoad)
        {
            if (Input.anyKeyDown)
                SceneManager.LoadScene(0);
        }
    }
}
