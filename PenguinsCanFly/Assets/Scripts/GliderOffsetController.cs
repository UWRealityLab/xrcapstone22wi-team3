using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GliderOffsetController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.5f));
    }
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetOffset();
    }

    private void SetOffset()
    {
        Debug.Log("SAVE:cameraOffset:" + Camera.main.transform.localPosition);
        transform.localPosition = new Vector3(0, Camera.main.transform.localPosition.y - 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("SAVE:cameraOffsetUpdate:" + Camera.main.transform.localPosition);

    }
}
