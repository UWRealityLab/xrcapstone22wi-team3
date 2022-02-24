using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    
    public Animator fadeAnimator;

    private bool hasBeenSelected;
    private string mScene;

    // Start is called before the first frame update
    void Start()
    {
        hasBeenSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeResetToScene(string sceneToLoad)
    {
        if (!hasBeenSelected)
        {
            fadeAnimator.SetTrigger("FadeTransition");
            mScene = sceneToLoad;
            hasBeenSelected = true;
        }
    }

    // Event called after end of FadeOutAnimation
    private void OnFadeOutCompleted()
    {
        // TODO: assumes we only have one scene
        hasBeenSelected = false;
        SceneManager.LoadScene(mScene);
    }

}
