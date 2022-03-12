using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFadeScript : MonoBehaviour
{
    private float fadePerSecond = 0.2f;
 
    private void Update() {
        var material = GetComponent<Renderer>().material;
        var color = material.color;

        if (color.a < 0.3)
        {
            material.color = new Color(color.r, color.g, color.b, color.a + (fadePerSecond * Time.deltaTime));
        }
    }
}
