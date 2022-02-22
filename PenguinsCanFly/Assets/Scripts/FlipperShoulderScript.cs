using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperShoulderScript : MonoBehaviour
{
    public Transform controller;
    public GameObject flipper;
    public GameObject gliderDirection;
    private Camera mainCamera;

    public bool isLeft;

    private Vector3 shoulderOffset;
    public const float shoulderXOffset = 0.11f;
    public const float shoulderYOffset = -0.2f;
    public const float shoulderZOffset = -0.05f;
    public const float backwardAngleThreshold = 160f;

    private const float flipperInitialZScale = 0.6f;
    private const float flipperInitialOffset = 0.398f;

    public const float flipperHandWidth = 0.12f;

    public Transform gliderHandlePos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        shoulderOffset = new Vector3(isLeft ? -shoulderXOffset : shoulderXOffset, shoulderYOffset, shoulderZOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // Compute rotation from forward for the shoulder offset
        Quaternion rotation;
        Vector3 gliderForward = gliderDirection.transform.forward;

        float cameraGliderAngle = Vector2.SignedAngle(new Vector2(this.mainCamera.transform.forward.x, mainCamera.transform.forward.z), new Vector2(gliderDirection.transform.forward.x, gliderDirection.transform.forward.z));
        if (Mathf.Abs(cameraGliderAngle) < backwardAngleThreshold)
        {
            float gliderFrontAngle = Vector2.SignedAngle(new Vector2(gliderForward.x, gliderForward.z), new Vector2(0, 1));
            rotation = Quaternion.AngleAxis(gliderFrontAngle, Vector3.up);
        }
        else
        {
            // If user starts to look backwards, flip their orientation 
            Vector3 gliderBack = gliderForward * -1;
            float gliderBackAngle = Vector2.SignedAngle(new Vector2(gliderBack.x, gliderBack.z), new Vector2(0, 1));
            rotation = Quaternion.AngleAxis(gliderBackAngle, Vector3.up);
        }

        Transform goalPosition = controller;
        if (gliderHandlePos != null)
        {
            goalPosition = gliderHandlePos;
        }

        // Set start point of shoulder
        Vector3 shoulderPosition = this.mainCamera.transform.position + (rotation * shoulderOffset);
        transform.position = shoulderPosition;

        // Point shoulder / flipper in controllers direction
        Vector3 controllerShoulderDiff = goalPosition.position - shoulderPosition;
        transform.forward = controllerShoulderDiff;

        // Add controller rotation in z dimension for some responsiveness
        this.transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, goalPosition.transform.eulerAngles.z);

        // Change Penguin Flipper length depending on how close controller is
        float flipperLength = controllerShoulderDiff.magnitude + flipperHandWidth;
        flipper.transform.localScale = new Vector3(flipper.transform.localScale.x, flipper.transform.localScale.y, flipperInitialZScale * flipperLength);
        flipper.transform.localPosition = new Vector3(0f, 0f, (flipperLength / 2) * flipperInitialOffset);
    }
}
