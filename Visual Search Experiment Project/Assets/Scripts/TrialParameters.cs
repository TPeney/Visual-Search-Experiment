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

        [HideInInspector]
        public string response;

        [HideInInspector]
        public bool responseCorrect;

        [HideInInspector]
        public bool targetShown;

        [Header("Parameters of the objects to be shown")]
        public VisualCue[] allCues;

    }

    void Start()
    {
        trial.name = gameObject.name;
        foreach (VisualCue cue in trial.allCues)
        {
            if (cue.isTarget)
            {
                trial.targetShown = true;
            }
        }
    }
}
