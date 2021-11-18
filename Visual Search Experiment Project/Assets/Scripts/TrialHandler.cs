using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class TrialHandler : MonoBehaviour
{
    [Tooltip("How many times each trial is repeated")]
    public int nReps = 1;
    public GameObject fixationCross;
    public float timeBetweenTrials;
    public Transform spawnedObjectHolder;
    public LocationHandler locationHandler;
    public bool saveResults;

    [HideInInspector]
    public TrialParameters.Trial[] trialList;


    [Header("Response Keys")]
    public string targetPresentKey;
    public string targetAbsentKey;

    private bool awaitingResponse = false;
    private string response;
    private bool trialRunning = false;
    private int trialNumber = 0;

    private DateTime trialStartTime;
    private DateTime timeOfResponse;

    // Start is called before the first frame update
    void Start()
    {
        trialList = CreateTrialList();

    }

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
            if (saveResults)
            {
                ExperimentHandler.SaveResults(trialList);
            }
            ExperimentHandler.ComponentComplete();
        }


        if (awaitingResponse)
        {
            GetInput();
        }
    }


    public TrialParameters.Trial[] CreateTrialList()
    {
        // Create list of initial trials
        GameObject[] tempTrialList = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            tempTrialList[i] = child.gameObject;
            i++;
        }

        // Creates duplicates based on nReps
        foreach (GameObject trial in tempTrialList)
        {
            for (int dup = 0; dup < nReps - 1; dup++)
            {
                Instantiate(trial, transform);
            }
        }

        // Create list of All Trials - Trial Objects from Children GameObjects
        trialList = new TrialParameters.Trial[transform.childCount];
        i = 0;
        foreach (Transform trial in transform)
        {
            trialList[i] = trial.gameObject.GetComponent<TrialParameters>().trial;
            i += 1;
        }

        // Assign pre-shuffle index; 
        for (int t = 0; t < trialList.Length; t++)
        {
            trialList[t].trialN = t + 1;
        }

        // Randomise order of trial list
        for (int t = 0; t < trialList.Length; t++)
        {
            TrialParameters.Trial tmp = trialList[t];
            int r = UnityEngine.Random.Range(t, trialList.Length);
            trialList[t] = trialList[r];
            trialList[r] = tmp;
        }

        // Assign post-shuffle index
        for (int t = 0; t < trialList.Length; t++)
        {
            trialList[t].orderShown = t + 1;
        }

        return trialList;

    }

    public void DrawStimuli(TrialParameters.Trial trial)
    {
        GameObject[] cueLocations = locationHandler.createCueLocationArray();

        // Draw each stimuli - location index based
        int spawnCount = 0;
        foreach (TrialParameters.VisualCue cue in trial.allCues)
        {
            if (cue.isTarget)
            {
                trial.targetShown = true;
            }
            for (int rep = 0; rep < cue.frequency; rep++)
            {
                GameObject temp = Instantiate(
                    cue.model,
                    cueLocations[spawnCount].transform.position,
                    cueLocations[spawnCount].transform.rotation * Quaternion.Euler(0f, 0f, cue.rotation)
                    );
                temp.transform.SetParent(spawnedObjectHolder, true); // Sets parent to a holder parent GameObject 

                spawnCount++;
            }
        }
    }

    public void ClearStimuli()
    {
        foreach (Transform child in spawnedObjectHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void GetInput()
    {
        bool respondedTargetPresent = Input.GetKeyDown(targetPresentKey);
        bool respondedTargetAbsent = Input.GetKeyDown(targetAbsentKey);

        if (respondedTargetPresent)
        {
            timeOfResponse = DateTime.Now;
            print("Responded target present");
            response = "Present";
            awaitingResponse = false;
        }

        else if (respondedTargetAbsent)
        {
            timeOfResponse = DateTime.Now;
            print("Responded target absent");
            response = "Absent";
            awaitingResponse = false;
        }
    }

    private void SaveTrialData(TrialParameters.Trial trial)
    {
        TimeSpan interval = timeOfResponse - trialStartTime;
        double reactionTime = interval.TotalMilliseconds;

        trial.reactionTime = reactionTime;
        print("RT = " + reactionTime.ToString());
        trial.response = response;

        if ((trial.targetShown && response == "Present") || (!trial.targetShown && response == "Absent"))
        {
            trial.trialPassed = true;

        }
        else
        {
            trial.trialPassed = false;
        }
    }

    IEnumerator RunTrial(TrialParameters.Trial trial)
    {
        trialRunning = true;
        if (fixationCross)
        {
            fixationCross.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(timeBetweenTrials);

        if (fixationCross)
        {
            fixationCross.SetActive(false);
        }

        trialStartTime = DateTime.Now;
        DrawStimuli(trial);

        awaitingResponse = true;

        while (awaitingResponse)
        {
            yield return null;
        }

        ClearStimuli();
        SaveTrialData(trial);
        trialNumber++;
        trialRunning = false;
    }
}

