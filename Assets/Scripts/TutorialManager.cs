using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 2.0f;
    float lowPassFilterFactor;
    Vector3 lowPassValue;
    StartMenuManager manager;

    // Start is called before the first frame update
    void Start()
    {
        //Setup shaking
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;

        manager = GetComponent<StartMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get input
        Vector3 acceleration = Input.acceleration;

        //Check for shake
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            //Send shake Event
            manager.StartGame();
        }

    }
}
