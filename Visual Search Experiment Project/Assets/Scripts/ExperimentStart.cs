using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperimentStart : MonoBehaviour
{
    public Button startButton;
    public TextMeshProUGUI sessionText;
    public TMP_InputField PID_Input;
    public GameObject warningText;

    private string session = DateTime.Now.ToString("dd.MM.yy_HH.mm");
    private string PID;
    private bool readyToStart;

    // Start is called before the first frame update
    void Start()
    {
        System.IO.Directory.CreateDirectory(Application.dataPath + "/Data");

        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(BeginExperimentPress);

        sessionText.SetText(session);
    }

    private void Update()
    {
        if (readyToStart)
        {
            ExperimentHandler.ComponentComplete();

        }
    }

    public void BeginExperimentPress()
    {
        PID = PID_Input.text;
        if (PID == "")
        {
            warningText.SetActive(true);
        }
        else if (PID != "")
        {
            ExperimentHandler.PID = PID;
            ExperimentHandler.session = session;
            readyToStart = true;
        }
    }
}
