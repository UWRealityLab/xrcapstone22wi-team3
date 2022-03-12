using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCliffHitDetector : MonoBehaviour
{
    public ResetManager resetManager;

    public AudioSource successAudioSource;

    public const string GliderTutorialScene = "GliderTutorialScene";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            successAudioSource.Play();
            resetManager.FadeResetToScene(GliderTutorialScene);
        }
    }
}