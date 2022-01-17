using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trial", fileName = "New Trial")]
public class TrialParametersSO : ScriptableObject
{
    // Trial Stimuli
    public string trialName;
    public int trialIndex;
    [SerializeField]
    public List<VisualCue> allVisualCues = new List<VisualCue>();

    [HideInInspector] public bool targetShown = false;

    // Order Info 
    [HideInInspector] public int trialN;
    [HideInInspector] public int orderShown;

    // Results
    [HideInInspector] public double reactionTime;
    [HideInInspector] public string response;
    [HideInInspector] public bool trialPassed;

    private void Awake()
    {
        trialName = this.name;
    }
}
