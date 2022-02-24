using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimationsScript : MonoBehaviour
{
    public List<ParticleSystem> winds;
    private Transform penguinXROTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        penguinXROTransform = GameController.Instance.gliderInfo.penguinXROTransform;
        foreach (ParticleSystem wind in winds)
        {
            wind.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.Instance.gliderInfo.isActiveAndEnabled)
        {
            return;
        }
        transform.position = penguinXROTransform.position + (Vector3.forward * 50);
        if (penguinXROTransform.position.y > 20)
        {
            foreach (ParticleSystem wind in winds)
            {
                if (wind.isPlaying)
                {
                    return;
                }
                wind.Play();
            }
        }
        else
        {
            foreach (ParticleSystem wind in winds)
            {
                if (wind.isStopped)
                {
                    return;
                }
                wind.Stop();
            }
        }
    }
}
