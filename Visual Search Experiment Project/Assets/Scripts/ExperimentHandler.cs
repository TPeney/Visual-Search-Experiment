using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentHandler : MonoBehaviour
{
    public TrialHandler TrialHandler;
    private TrialParameters.Trial[] trialList;

    // Start is called before the first frame update
    void Start()
    {
        trialList = TrialHandler.createTrialList();
        runTrial(trialList[0]);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void runTrial(TrialParameters.Trial trial)
    {
        TrialHandler.drawStimuli(trial);
    }

}
