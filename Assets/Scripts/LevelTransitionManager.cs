using System.Collections;
using TMPro;
using UnityEngine;

public class LevelTransitionManager : MonoBehaviour
{
    private Animator transitionAnimator;
    public TextMeshProUGUI levelText;
    public float transitionTime = 1.0f;

    private void Start()
    {
        transitionAnimator = GetComponent<Animator>();

        if (transitionAnimator != null)
        {
            StartCoroutine(FadeOut());
        }
    }

    public IEnumerator FadeIn(string levelName)
    {
        levelText.text = levelName;
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("Start");
        }

        yield return new WaitForSeconds(transitionTime);
    }

    public IEnumerator FadeOut()
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("End");
        }

        yield return new WaitForSeconds(transitionTime);
    }

    public IEnumerator KillAll()
    {
        levelText.text = "KILL ALL BEFORE";

        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("KillAll");
        }

        yield return new WaitForSeconds(transitionTime);
    }
}
