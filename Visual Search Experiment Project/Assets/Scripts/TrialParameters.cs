using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialParameters : MonoBehaviour
{
    public Trial trial;

    [System.Serializable]
    public class VisualCue
    {
        public string name;
        public GameObject model;
        public int frequency;
        public float rotation;
        public bool isTarget;
    }

    [System.Serializable]
    public class Trial
    {
        [HideInInspector]
        public string name;

        [HideInInspector]
        public int trialN;

        [HideInInspector]
        public int orderShown;

        [Header("Parameters of the objects to be shown")]
        public VisualCue[] allCues;

        [HideInInspector]
        public string response;

        [HideInInspector]
        public bool targetShown; // Set in TrialHandler.DrawStimuli() 

        [HideInInspector]
        public bool trialPassed;

        [HideInInspector]
        public double reactionTime;

    }

    void Start()
    {
        trial.name = gameObject.name;
    }
}
