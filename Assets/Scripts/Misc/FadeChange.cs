using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeChange : MonoBehaviour
{
    public Animator animator;

    private int levelToLoad;

    // Update is called once per frame
    private void Start()
    {
        animator = GetComponent<Animator>();
        FadeScreen(false);
    }

    public void FadeScreen(bool i)
    {
        if (i != true)
        {
            animator.SetBool("Fade", false);
        }
        else
        {
            animator.SetBool("Fade", true);
        }
    }

/*    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }*/
}
