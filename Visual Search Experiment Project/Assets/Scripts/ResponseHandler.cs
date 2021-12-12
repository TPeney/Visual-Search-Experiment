using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResponseHandler : MonoBehaviour
{
    TrialHandler trialHandler;
    ExperimentHandler experimentHandler;

    void Start()
    {
        experimentHandler = GetComponent<ExperimentHandler>();
    }

    private void Update()
    {
        trialHandler = experimentHandler.currentTrialHandler;
    }

    void OnRespondTargetPresent(InputValue value)
    {
        if (trialHandler.awaitingResponse)
        {
            trialHandler.timeOfResponse = DateTime.Now;
            trialHandler.response = "Present";
            trialHandler.awaitingResponse = false;
            Debug.Log("Present Pressed");
        }
    }

    void OnRespondTargetAbsent(InputValue value)
    {
        if (trialHandler.awaitingResponse)
        {
            trialHandler.timeOfResponse = DateTime.Now;
            trialHandler.response = "Absent";
            trialHandler.awaitingResponse = false;
            Debug.Log("Absent Pressed");

        }
    }

    void OnContinue(InputValue value)
    {
        if (experimentHandler.awaitingResponse)
        {
            experimentHandler.awaitingResponse = false;
            experimentHandler.ComponentComplete();
        }
    }
}
