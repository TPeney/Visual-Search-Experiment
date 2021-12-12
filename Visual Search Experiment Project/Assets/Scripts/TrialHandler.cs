using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;


public class TrialHandler : MonoBehaviour
{
    [Header("Trial Handler Parameters")]
    [Tooltip("How many times each trial is repeated.")]
    public int nReps = 1;

    [Tooltip("Include a fixation cross before trial starts, or leave blank for none.")]
    public GameObject fixationCross;
    public float fixationCrossDuration;

    [Tooltip("Gap between trial presentation in seconds")]
    public float timeBetweenTrials;
    public bool saveResults;

    public Transform instantiatedStimuliContainer;
    LocationHandler locationHandler;

    [Tooltip("Add each trial as a TrialParameter SO")]
    [SerializeField]
    List<TrialParametersSO> trialList = new List<TrialParametersSO>();
    private int currentTrialIndex = 0;

    // Fields to handle response data for each trial
    private bool trialRunning = false;

    private DateTime currentTrialStartTime;
    [HideInInspector] public bool awaitingResponse = false;
    [HideInInspector] public string response;
    [HideInInspector] public DateTime timeOfResponse;

    ExperimentHandler experimentHandler;

    private void Awake()
    {
        experimentHandler = FindObjectOfType<ExperimentHandler>();
    }

    void Start()
    {
        trialList = CreateTrialList();
        locationHandler = GetComponentInChildren<LocationHandler>();
    }

    void Update()
    {
        // If a trial isn't currently active - run a trial 
        if (currentTrialIndex < trialList.Count)
        {
            if (!trialRunning)
            {
                StartCoroutine(RunTrial(trialList[currentTrialIndex]));
            }
        }
        else
        {
            if (saveResults)
            {
                experimentHandler.SaveResults(trialList);
            }
            experimentHandler.ComponentComplete();
        }
    }

    public List<TrialParametersSO> CreateTrialList()
    {
        /* Creates a list of Trial SO's based on initial given trials 
         * and desired amount of nReps. Sets the index and order shown
         fields in each Trial SO. */

        // Populate a list based on given trials and nReps
        List <TrialParametersSO> tempTrialList = trialList;
        List<TrialParametersSO> fullTrialList = Enumerable
            .Range(0, nReps)
            .SelectMany(e => trialList)
            .ToList();

        // Assign pre-shuffle index; 
        for (int t = 0; t < fullTrialList.Count; t++)
        {
            fullTrialList[t].trialN = t + 1;
        }

        // Randomise order of trial list
        for (int t = 0; t < fullTrialList.Count; t++)
        {
            TrialParametersSO tmp = fullTrialList[t];
            int r = UnityEngine.Random.Range(t, fullTrialList.Count);
            fullTrialList[t] = fullTrialList[r];
            fullTrialList[r] = tmp;
        }

        // Assign post-shuffle index
        for (int t = 0; t < fullTrialList.Count; t++)
        {
            fullTrialList[t].orderShown = t + 1;
        }

        return fullTrialList;
    }

    public void DrawStimuli(TrialParametersSO trial)
    {
        GameObject[] cueLocations = locationHandler.createCueLocationArray();

        // Draw each stimuli - location index based
        int spawnCount = 0;
        foreach (VisualCue cue in trial.allVisualCues)
        {
            if (cue.isTarget)
            {
                trial.targetShown = true;
            }
            else
            {
                trial.targetShown = false;
            }

            for (int rep = 0; rep < cue.count; rep++)
            {
                GameObject temp = Instantiate(
                    cue.model,
                    cueLocations[spawnCount].transform.position,
                    cueLocations[spawnCount].transform.rotation * Quaternion.Euler(0f, 0f, cue.rotation)
                    );
                temp.transform.SetParent(instantiatedStimuliContainer, true); // Sets parent to a holder parent GameObject 

                spawnCount++;
            }
        }
    }

    public void ClearStimuli()
    {
        foreach (Transform child in instantiatedStimuliContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void SaveTrialData(TrialParametersSO trial)
    {
        TimeSpan interval = timeOfResponse - currentTrialStartTime;
        double reactionTime = interval.TotalMilliseconds;

        trial.reactionTime = reactionTime;
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

    IEnumerator RunTrial(TrialParametersSO trial)
    {
        trialRunning = true;

        yield return new WaitForSecondsRealtime(timeBetweenTrials);

        if (fixationCross)
        {
            fixationCross.SetActive(true);
            yield return new WaitForSecondsRealtime(fixationCrossDuration);
            fixationCross.SetActive(false);
        }

        DrawStimuli(trial);
        currentTrialStartTime = DateTime.Now;
        awaitingResponse = true;

        while (awaitingResponse)
        {
            yield return null;
        }

        // End of Trial Cleanup 
        ClearStimuli();
        SaveTrialData(trial);
        currentTrialIndex++;
        trialRunning = false;
    }
}

