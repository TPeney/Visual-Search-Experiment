using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;
using TMPro;

public class ExperimentStart : MonoBehaviour
{
    ExperimentHandler experimentHandler;

    // GUI Components
    public TMP_Dropdown conditionSelector1; // UI Dropdown to select condition - !! Order must match the order of the condition list on ExperimentHandler
    public TMP_Dropdown conditionSelector2;
    public TMP_Dropdown conditionSelector3;

    public TMP_InputField PID_Input;
    public TextMeshProUGUI sessionText;
    public Button startButton;
    public TextMeshProUGUI warningText;

    // Local vars
    private string errorText;
    private readonly string session = DateTime.Now.ToString("dd.MM.yy_HH-mm");
    private string PID;
    List<int> conditionOrder = new List<int>();
    private bool readyToStart = false;


    private void Awake()
    {
        experimentHandler = FindObjectOfType<ExperimentHandler>();
        /*XRGeneralSettings.Instance.Manager.StopSubsystems();*/ // Disable VR so GUI works on desktop
    }

    void Start()
    {
        // Settup Start Button
        Button btn = startButton.GetComponent<Button>(); // Create Reference
        btn.onClick.AddListener(BeginExperimentPress); // Run function on every click

        // Set Session Text UI to display current DateTime
        sessionText.SetText(session);
    }

    private void Update()
    {
        if (readyToStart)
        {
            SceneManager.LoadScene(ExperimentHandler.currentCondition + 1); // +1 as 0 is starting scene 
            ExperimentHandler.experimentStarted = true;
        }   
    }

    public void BeginExperimentPress() // When Button Pressed:
    {
        bool inputValid = CheckInput(); 

        if (inputValid)
        {
            SetExperimentParameters();
            CreateDirectories();

            XRGeneralSettings.Instance.Manager.StartSubsystems(); // Initialize VR 

            readyToStart = true; // At next frame (Update) load the experiment scene
        }
        else 
        {
            warningText.text = errorText;
        }
    }

    private bool CheckInput() // Type-check input 
    {
        PID = PID_Input.text; // Sets Temp PID based on InputBox current text
        if (!Int32.TryParse(PID, out int i)) // If PID can't be converted to a number, Display a warning if PID box has spaces/is empty
        {
            errorText = "Input a valid PID";
            return false;
        }

        // Check for all unique items in condition order list
        conditionOrder.Add(conditionSelector1.value);
        conditionOrder.Add(conditionSelector2.value);
        conditionOrder.Add(conditionSelector3.value);

        if (conditionOrder.Distinct().Count() != conditionOrder.Count) 
        {
            errorText = "A duplicate condition has been inputted.";
            conditionOrder.Clear();
            return false;
        }

        errorText = "";
        return true;
    }
     
    private void SetExperimentParameters() // Set metadata variables of ExperimentHandler
    {
        /* Set Condition Based on Dropdown Value
         * 0 = 2D
         * 1 = VR Int
         * 2 = VR Full */

        ExperimentHandler.conditionOrder = conditionOrder;
        ExperimentHandler.currentCondition = conditionOrder[0];
        ExperimentHandler.PID = PID;
        ExperimentHandler.session = session;
    }

    private void CreateDirectories() // Ensure Data Directories have been created 
    {
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Data"); // Overall
    }
}

