using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    
    public Animator fadeAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeAndReset()
    {
        fadeAnimator.SetTrigger("FadeTransition");
    }

    // Event called after end of FadeOutAnimation
    private void OnFadeOutCompleted()
    {
        // TODO: assumes we only have one scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
