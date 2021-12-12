using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionText : MonoBehaviour
{
    ExperimentHandler experimentHandler;

    private void Start()
    {
        experimentHandler = FindObjectOfType<ExperimentHandler>();
        experimentHandler.awaitingResponse = true;
    }
}
