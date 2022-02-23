using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadSceneAsyncScript : MonoBehaviour
{
    public string sceneToLoad;

    public void onButtonClickLoadSceneAsync()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // Wait until the asynchronous scene fully loads
        // TODO: Display a black loading screen so jump isn't as apparent? Or loading overlay
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
