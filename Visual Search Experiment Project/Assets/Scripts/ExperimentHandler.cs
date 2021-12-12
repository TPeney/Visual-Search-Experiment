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

    [HideInInspector] public static bool experimentStarted;
    private int currentComponent = 0;
    public bool componentRunning = false;
    public bool awaitingResponse = false;
    bool activeComponent = false;
    [HideInInspector] public TrialHandler currentTrialHandler;

    static ExperimentHandler instance;

    private void Awake()
    {
        //ManageSingleton();
        SetUpComponentList();
    }
        
    private void ManageSingleton()
    { // Create Singleton
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void SetUpComponentList()
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
        if (!activeComponent && experimentStarted)
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

    public void ComponentComplete()
    {
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

    IEnumerator ShowElement()
    {
        activeComponent = true;
        componentRunning = true;
        experimentComponents[currentComponent].SetActive(true);

        if (experimentComponents[currentComponent].GetComponent<TrialHandler>() != null)
        {
            currentTrialHandler = experimentComponents[currentComponent].GetComponent<TrialHandler>();
        }

        while (componentRunning)
        {
            yield return null;
        }

        experimentComponents[currentComponent].SetActive(false);
        currentComponent++;
        activeComponent = false;
    }
}





