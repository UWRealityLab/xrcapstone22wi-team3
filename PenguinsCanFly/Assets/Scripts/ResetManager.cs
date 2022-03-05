using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

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
        bool menuButtonValue;
        DeviceManager.Instance.leftHandDevice.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonValue);
        if (menuButtonValue)
        {
            FadeResetToScene("JamesMenuScene");
        }
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
        hasBeenSelected = false;
        SceneManager.LoadScene(mScene);
    }

}
