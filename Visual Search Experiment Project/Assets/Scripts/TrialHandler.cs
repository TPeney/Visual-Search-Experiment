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

    [Header("Break Adjustment")]
    [Tooltip("Leave blank for no breaks.")]
    [SerializeField] GameObject breakScreen;

    [Tooltip("How many total breaks? Will be inserted evenly across all trials.")]
    [SerializeField] int numBreaks;

    [HideInInspector] public bool takingBreak = false;
    [HideInInspector] List<int> breakIndices = new List<int>();


    // Fields to handle response data for each trial
    private bool trialRunning = false;

    private DateTime currentTrialStartTime;
    [HideInInspector] public bool awaitingResponse = false;
    [HideInInspector] public string response;
    [HideInInspector] public DateTime timeOfResponse;

    [HideInInspector] private Vector3 localScale;

    ExperimentHandler experimentHandler;

    private void Awake()
    {
        experimentHandler = FindObjectOfType<ExperimentHandler>();
    }

    void Start()
    {
        trialList = CreateTrialList();
        locationHandler = GetComponentInChildren<LocationHandler>();
        localScale = experimentHandler.gameObject.transform.localScale;

        if (breakScreen != null)
        {
            breakScreen.SetActive(false); // Ensure break screen is initially turned off
            DetermineBreakIndices();
        }
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
        List<TrialParametersSO> fullTrialList = new List<TrialParametersSO>();
        
        for (int i = 0; i < nReps; i++)
        {
            foreach (TrialParametersSO trial in tempTrialList.ToList())
            {
                TrialParametersSO tempTrial = Instantiate(trial);
                fullTrialList.Add(tempTrial);
            }
        }

        // Assign pre-shuffle index & condition;    
        for (int t = 0; t < fullTrialList.Count; t++)
        {
            fullTrialList[t].trialN = t + 1;
            fullTrialList[t].condition = ExperimentHandler.condition;
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

    private void DetermineBreakIndices()
    {
        // Calculate the interval for breaks to occur at given the total trial count and desired number of breaks.
        int spacer = (int)Math.Ceiling((double) trialList.Count / (numBreaks + 1)); 

        // Populate list with indeces at which breaks are to occur.
        for (int i = 1; spacer * i < trialList.Count; i++)
        {
            breakIndices.Add(i * spacer);
        }
    }

    private void RunBreak()
    {
        takingBreak = true;
        breakScreen.SetActive(true); // Set to false by response handler
    }

    public void DrawStimuli(TrialParametersSO trial)
    {
        GameObject[] cueLocations = locationHandler.createCueLocationArray();

        // Draw each stimuli - location index based
        int spawnCount = 0;
        foreach (VisualCue cue in trial.allVisualCues)
        {
            for (int rep = 0; rep < cue.count; rep++)
            {
                Vector3 rotation;

                // If custom rotation on the z-axis is given, use that, otherwise use prefab default
                if (cue.zRotation != 0)
                {
                    rotation = new Vector3(0f, 0f, cue.zRotation);
                }
                else
                {
                    rotation = cue.model.transform.rotation.eulerAngles;
                }

                GameObject temp = Instantiate(
                  cue.model,
                  cueLocations[spawnCount].transform.position,
                  cueLocations[spawnCount].transform.rotation * Quaternion.Euler(rotation),
                  instantiatedStimuliContainer
                  );

                if (cue.isTarget)
                {
                    trial.targetShown = true;
                    trial.targLocX = temp.transform.localPosition.x;
                    trial.targLocY = temp.transform.localPosition.y;
                }
                else
                {
                    trial.targetShown = false;
                }
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

        // Check for break screen
        if (breakScreen != null)
        {
            if (breakIndices.Contains(currentTrialIndex))
            {
                RunBreak();
                while (takingBreak) // Set to false by response handler
                {
                    yield return null;
                }
                breakScreen.SetActive(false);
            }
        }

        // Run Main Trial
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

