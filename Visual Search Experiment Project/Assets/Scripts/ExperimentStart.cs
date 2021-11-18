using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using TMPro;

public class ExperimentStart : MonoBehaviour
{
    public ExperimentHandler ExperimentHandler;
    public TMP_Dropdown conditionSelector; // UI Dropdown to select condition - !! Order must match the order of the condition list on ExperimentHandler
    public TMP_InputField PID_Input;
    public TextMeshProUGUI sessionText;
    public Button startButton;

    public GameObject warningText;

    private readonly string session = DateTime.Now.ToString("dd.MM.yy_HH-mm");
    private string PID;
    private Condition condition;
    private bool readyToStart;

    // Start is called before the first frame update
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
            ExperimentHandler.ComponentComplete();
        }
    }

    public void BeginExperimentPress() // When Button Pressed:
    {
        PID = PID_Input.text; // Sets Temp PID based on InputBox current text

        if (Int32.TryParse(PID, out int i)) // If PID is a valid int:
        {
            // Set Condition (Object) Based of Dropdown Value
            int conditionIndex = conditionSelector.value;
            condition = ExperimentHandler.conditions[conditionIndex];

            // Set metadata variables of ExperimentHandler
            ExperimentHandler.condition = condition;
            ExperimentHandler.PID = PID;
            ExperimentHandler.session = session;

            // Ensure Data Directories have been created 
            System.IO.Directory.CreateDirectory(Application.dataPath + "/Data"); // Overall
            System.IO.Directory.CreateDirectory(Application.dataPath + $"/Data/{ExperimentHandler.condition.name}"); // Current Condition

            // Disable VR Mode if needed
            if (condition.name == "2D")
            {
                XRGeneralSettings.Instance.Manager.DeinitializeLoader(); // Stop VR
            }
            else
            {
                ExperimentHandler.conditions[0].camera.SetActive(false); // Turn off the 2D Camera
                condition.camera.SetActive(true); // Turn on XR Rig
            }

            // At next frame (Update) - Tell ExperimentHandler to move on
            readyToStart = true;

        }
        else // If PID can't be converted to a number:
        {
            warningText.SetActive(true); // Display a warning if PID box has spaces/is empty
        }
    }
}
