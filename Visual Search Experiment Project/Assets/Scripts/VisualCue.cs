using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisualCue 
{
    public string label;
    public GameObject model;
    public int count;
    public float zRotation;
    public bool isTarget = false;
}
