using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;
using TMPro;

public class ExperimentStart : MonoBehaviour
{
    ExperimentHandler experimentHandler;
    public TMP_Dropdown conditionSelector; // UI Dropdown to select condition - !! Order must match the order of the condition list on ExperimentHandler
    public TMP_InputField PID_Input;
    public TextMeshProUGUI sessionText;
    public Button startButton;
    public GameObject warningText;

    private readonly string session = DateTime.Now.ToString("dd.MM.yy_HH-mm");
    private string PID;
    private bool readyToStart = false;

    private void Awake()
    {
        experimentHandler = FindObjectOfType<ExperimentHandler>();
    }

    void Start()
    {
        // Settup Start Button
        Button btn = startButton.GetComponent<Button>(); // Create Reference
        btn.onClick.AddListener(BeginExperimentPress); // When clicked - Run BeginExperimentPress Function

        // Set Session Text UI to display current DateTime
        sessionText.SetText(session);
    }

    private void Update()
    {
        if (readyToStart)
        {
            SceneManager.LoadScene(experimentHandler.condition + 1);
            experimentHandler.experimentStarted = true;
        }   
    }

    public void BeginExperimentPress() // When Button Pressed:
    {
        // Sets Temp PID based on InputBox current text
        PID = PID_Input.text;

        if (Int32.TryParse(PID, out int i)) // If PID is a valid int:
        {
            SetExperimentParameters();
            CreateDirectories();

            // Disable VR Mode if needed
            if (experimentHandler.condition == 0)
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader(); // Stop VR
            }

            // At next frame (Update) - Tell ExperimentHandler to move on
            readyToStart = true;
        }
        else // If PID can't be converted to a number:
        {
            warningText.SetActive(true); // Display a warning if PID box has spaces/is empty
        }
    }
    private void SetExperimentParameters()
    {
        // Set metadata variables of ExperimentHandler

        /* Set Condition (Object) Based of Dropdown Value
         * 0 = 2D
         * 1 = VR Int
         * 2 = VR Full */
        int conditionIndex = conditionSelector.value;
        experimentHandler.condition = conditionIndex;
        experimentHandler.PID = PID;
        experimentHandler.session = session;
    }
    private void CreateDirectories()
    {
        // Ensure Data Directories have been created 
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Data"); // Overall
        System.IO.Directory.CreateDirectory(Application.dataPath + $"/Data/{experimentHandler.condition}"); // Current Condition
    }
}

