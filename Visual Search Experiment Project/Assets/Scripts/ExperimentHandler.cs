using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentHandler : MonoBehaviour
{
    public TrialHandler TrialHandler;
    private TrialParameters.Trial[] trialList;

    [Header("Response Keys")]
    public string targetPresentKey;
    public string targetAbsentKey;

    private bool awaitingResponse = false;
    private string response;
    private bool trialRunning = false;
    private int trialNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        trialList = TrialHandler.createTrialList();
        //for (int trialNumber = 0; trialNumber < trialList.Length; trialNumber++)
        //{

        //}

    }

    // Update is called once per frame
    void Update()
    {
        // If a trial isn't currently active - run a trial 
        if (trialNumber < trialList.Length)
        {
            if (!trialRunning)
            {
                StartCoroutine(RunTrial(trialList[trialNumber]));
            }
        }
        
        else
        {
            //print("Experiment end");
        }


        if (awaitingResponse)
        {
            bool respondedTargetPresent = Input.GetKeyDown(targetPresentKey);
            bool respondedTargetAbsent = Input.GetKeyDown(targetAbsentKey);

            if (respondedTargetPresent)
            {
                print("Responded target present");
                response = "Target Present";
                awaitingResponse = false;

            }

            else if (respondedTargetAbsent)
            {
                print("Responded target absent");
                response = "Target Absent";
                awaitingResponse = false;
            }
        }
    }

    IEnumerator RunTrial(TrialParameters.Trial trial)
    {
        trialRunning = true;
        TrialHandler.drawStimuli(trial);
        // log start time
        awaitingResponse = true;

        while (awaitingResponse)
        {
            yield return null;
        }

        TrialHandler.clearStimuli();

        trialNumber++;
        trialRunning = false;
    }


}