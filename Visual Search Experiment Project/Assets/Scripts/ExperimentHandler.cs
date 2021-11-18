using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentHandler : MonoBehaviour
{
    public Condition[] conditions;

    public GameObject[] experimentComponents;

    [HideInInspector]
    public static string PID;
    [HideInInspector]
    public static string session;
    [HideInInspector]
    public static Condition condition;

    private static int currentComponent = 0;
    private static bool componentRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject component in experimentComponents)
        {
            component.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!componentRunning)
        {
            if (currentComponent < experimentComponents.Length)
            {
                StartCoroutine(ShowElement());
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public static void ComponentComplete()
    {
        componentRunning = false;
    }

    public static void SaveResults(TrialParameters.Trial[] trialList)
    {
        string path = Application.dataPath +
                    $"/Data/{condition.name}/" +
                    $"Participant {PID}" +
                    $"_Visual_Search_Task" +
                    $"_({session}).csv";

        Csv.CsvUtil.SaveObjects(trialList, path);
    }

    IEnumerator ShowElement()
    {
        componentRunning=true;
        experimentComponents[currentComponent].SetActive(true);

        while (componentRunning)
        {
            yield return null;
        }

        experimentComponents[currentComponent].SetActive(false);

        currentComponent++;
    }
}





