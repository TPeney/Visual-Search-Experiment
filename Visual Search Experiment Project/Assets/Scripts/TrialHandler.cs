using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TrialHandler : MonoBehaviour
{
    [Tooltip("How many times each trial is repeated")]
    public int nReps = 1;
    public Transform spawnedObjectHolder;

    public locationHandler locationHandler;
    public GameObject fixationCross;

    [HideInInspector]
    public TrialParameters.Trial[] trialList;

    // Start is called before the first frame update
    void Start()
    {
    }

    public TrialParameters.Trial[] createTrialList()
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
            int r = Random.Range(t, trialList.Length);
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

    public void drawStimuli(TrialParameters.Trial trial)
    {
        GameObject[] cueLocations = locationHandler.createCueLocationArray();

        // Draw each stimuli - location index based
        int spawnCount = 0;
        foreach (TrialParameters.VisualCue cue in trial.allCues)
        {
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

    public void clearStimuli()
    {
        foreach (Transform child in spawnedObjectHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}

