using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TrialHandler : MonoBehaviour
{
    [Tooltip("How many times each trial is repeated")]
    public int nReps = 1;
    
    // Start is called before the first frame update
    void Start()
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

        // Create list of All Trials - Children GameObjects
        TrialParameters.Trial[] trialList = new TrialParameters.Trial[transform.childCount];
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

        foreach (TrialParameters.Trial trial in trialList)
        {
            //print(trial.name + " " + trial.trialN);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

   
 }

