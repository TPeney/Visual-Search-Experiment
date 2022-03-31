using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentHandler : MonoBehaviour
{
    public List<GameObject> experimentComponents = new List<GameObject>();

    // Experiment-Wide Parameters 
    [HideInInspector] public static string PID;
    [HideInInspector] public static string session;
    [HideInInspector] public static int condition;
    [HideInInspector] public static string conditionName;

    // Running Status Parameters 
    [HideInInspector] public TrialHandler currentTrialHandler; // Used by ResponseHandler
    [HideInInspector] public static bool experimentStarted;
    private int currentComponent = 0;
    public bool componentRunning = false;
    public bool awaitingResponse = false;

    //static ExperimentHandler instance;

    private void Awake()
    {
        //ManageSingleton();
        SetUpComponentList();
    }
    private void SetUpComponentList() // All components should be added as children to a parent with this script
    {
        foreach (Transform component in gameObject.GetComponentInChildren<Transform>())
        {
            experimentComponents.Add(component.gameObject);
        }

        foreach (GameObject component in experimentComponents)
        {
            component.SetActive(false);
        }
    }
    
    void Update()
    {
        if (!componentRunning && experimentStarted)
        {
            if (currentComponent < experimentComponents.Count)
            {
                StartCoroutine(ShowElement());
            }
            else
            {
                Application.Quit();
            }
        }
    }

    //private void ManageSingleton()
    //{ // Create Singleton
    //    if (instance != null)
    //    {
    //        gameObject.SetActive(false);
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //}
 
    IEnumerator ShowElement()
    {
        componentRunning = true;
        experimentComponents[currentComponent].SetActive(true);

        currentTrialHandler = experimentComponents[currentComponent].GetComponent<TrialHandler>(); // null if no trials running

        while (componentRunning) // Set to false by ResponseHandler or TrialHandler - calls ComponentComplete()
        {
            yield return null;
        }
    }

    public void ComponentComplete()
    {
        experimentComponents[currentComponent].SetActive(false);
        currentComponent++;
        componentRunning = false;
    }

    public void SaveResults(List<TrialParametersSO> trialList)
    {
        string path = Application.dataPath +
                    $"/Data/{condition}/" +
                    $"Participant {PID}" +
                    $"_Visual_Search_Task" +
                    $"_({session}).csv";

        Csv.CsvUtil.SaveObjects(trialList, path);
    }
}





