using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DistractorHandler : MonoBehaviour
{

    public int numDistractors;


    // Start is called before the first frame update
    void Start()
    {

       // Create list of Children GameObjects - Each Distractor
        GameObject[] distractors = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            distractors[i] = child.gameObject;
            i += 1;
        }

        int allDistractors = distractors.Length;

        // Hide all distractors at start 
        foreach (GameObject distractor in distractors)
        {
            distractor.SetActive(false);
        }

        for (int t = 0; t < allDistractors; t++)
        {
            GameObject tmp = distractors[t];
            int r = Random.Range(t, allDistractors);
            distractors[t] = distractors[r];
            distractors[r] = tmp;
        }

        for (int d = 0; d < numDistractors; d++)
        {
            distractors[d].SetActive(true);
        }

       }
 

    

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
