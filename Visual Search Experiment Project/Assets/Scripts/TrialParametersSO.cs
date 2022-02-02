using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trial", fileName = "New Trial")]
public class TrialParametersSO : ScriptableObject
{
    [HideInInspector] public int condition = -1;

    // Trial Stimuli
    public string trialName;
    [SerializeField] public List<VisualCue> allVisualCues = new List<VisualCue>();


    // Order Info 
    [HideInInspector] public int trialN;
    [HideInInspector] public int orderShown;
    public int arraySize;

    // Results
    [HideInInspector] public bool targetShown = false;
    [HideInInspector] public string response;
    [HideInInspector] public bool trialPassed;
    [HideInInspector] public double reactionTime;
    [HideInInspector] public float targLocX;
    [HideInInspector] public float targLocY;



    private void Awake()
    {
        trialName = this.name;
        arraySize = 0;
        foreach (var cue in allVisualCues)
        {
            arraySize += cue.count;
        }
    }
}
