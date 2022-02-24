using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadSceneOnPenguinSelectScript : MonoBehaviour
{
    public string sceneToLoad;
    public ResetManager resetManager;

    public void onPenguinSelectFade()
    {
        resetManager.FadeResetToScene(sceneToLoad);
    }
}
