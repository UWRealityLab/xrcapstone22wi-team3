using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    
    public Animator fadeAnimator;
    private string scene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeResetToMenu()
    {
        fadeAnimator.SetTrigger("FadeTransition");
        scene = "JamesMenuScene";
    }

    public void FadeResetToLaunch()
    {
        fadeAnimator.SetTrigger("FadeTransition");
        scene = "MasterScene";
    }

    // Event called after end of FadeOutAnimation
    private void OnFadeOutCompleted()
    {
        // TODO: assumes we only have one scene
        SceneManager.LoadScene(scene);
    }

}
