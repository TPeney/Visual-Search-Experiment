using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    public string continueKey;
  
    // Update is called once per frame
    void Update()
    {
        bool continueKeyPressed = Input.GetKeyDown(continueKey);
        if (continueKeyPressed)
        {
            ExperimentHandler.ComponentComplete();
        }
    }
}
