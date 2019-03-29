using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static float MaxXCamera { get => instance.maxXCamera.transform.position.x; }
    [SerializeField] private Transform maxXCamera;

    private void Awake()
    {
        if (!instance)
            instance = this;

        Character.CanMove = true;
    }

    public static void LoseGame()
    {
        if (!instance)
            return;

        instance.StartCoroutine(instance.LoadScene(
            SceneManager.GetActiveScene().buildIndex));
    }

    public static void Win()
    {
        if (!instance)
            return;

        Character.CanMove = false;
        int index = SceneManager.GetActiveScene().buildIndex +1;
        if (index >= SceneManager.sceneCountInBuildSettings)
            index = 0;

        instance.StartCoroutine(instance.LoadScene(index));
    }

    IEnumerator LoadScene(int index, float timer = 2)
    {
        InGameUI.FadeOut();
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(index);
    }
}
