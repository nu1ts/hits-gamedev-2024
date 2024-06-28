using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController instance;
    private LevelTransitionManager transitionManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            transitionManager = GetComponentInChildren<LevelTransitionManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FirstLevel()
    {
        StartCoroutine(Transition(1, "0 - 1"));
    }

    public void NextLevel()
    {
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex + 1, "0 - " + (SceneManager.GetActiveScene().buildIndex + 1)));
    }

    public void RestartLevel()
    {
        StartCoroutine(Transition(SceneManager.GetActiveScene().buildIndex, "0 - " + SceneManager.GetActiveScene().buildIndex));
    }

    public void NeedToKillAll()
    {
        StartCoroutine(TransitionKillAll());
    }

    private IEnumerator TransitionKillAll()
    {
        yield return transitionManager.KillAll();
    }

    private IEnumerator Transition(int sceneIndex, string levelName)
    {
        if (transitionManager != null)
        {
            yield return transitionManager.FadeIn(levelName);
        }

        SceneManager.LoadSceneAsync(sceneIndex);

        if (transitionManager != null)
        {
            yield return transitionManager.FadeOut();
        }
    }
}
