using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ExportData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExportData(/*TrialParameters.Trial[] trialList*/)
    {
        //string cwd = Directory.GetCurrentDirectory();
        //string filePath = cwd + "\\test.cvs";
        //print(filePath);
        //StreamWriter writer = new StreamWriter(filePath);

        //writer.WriteLine("Test, Test");
        //writer.Flush();
        //writer.Close();
    }
}
