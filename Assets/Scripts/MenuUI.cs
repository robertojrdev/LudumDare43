using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    Animator anim;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        anim = GetComponent<Animator>();

        startButton.onClick.AddListener(() =>
        StartCoroutine(StartGame()));

        exitButton.onClick.AddListener(Application.Quit);
    }

    IEnumerator StartGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        anim.SetTrigger("Fade");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
