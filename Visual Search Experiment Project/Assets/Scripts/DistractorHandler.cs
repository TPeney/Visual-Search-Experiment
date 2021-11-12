using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DistractorHandler : MonoBehaviour
{

    public int numDistractors; // to-do - Set from current trial
    public Distractor[] distractorTypes;

    // Class for storing distractor parameters
    [System.Serializable]
    public class Distractor
    {
        public GameObject model;
        public int frequency;
        public float rotation;
    }
     
    // Start is called before the first frame update
    void Start()
    {

       // Create list of Children GameObjects - Each Distractor
        GameObject[] distractorLocations = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            distractorLocations[i] = child.gameObject;
            i += 1;
        }

        int allDistractors = distractorLocations.Length;

        // Hide all distractors at start 
        //foreach (GameObject distractor in distractorLocations)
        //{
        //    distractor.SetActive(false);
        //}

        for (int t = 0; t < allDistractors; t++)
        {
            GameObject tmp = distractorLocations[t];
            int r = Random.Range(t, allDistractors);
            distractorLocations[t] = distractorLocations[r];
            distractorLocations[r] = tmp;
        }

        //for (int d = 0; d < numDistractors; d++)
        //{
        //    distractorLocations[d].SetActive(true);
        //}

        int spawnCount = 0;
        foreach (Distractor distractor in distractorTypes)
        {
            for (int rep = 0; rep < distractor.frequency; rep++)
            {
                Instantiate(distractor.model, distractorLocations[spawnCount].transform); //set quaternion

                spawnCount++;
            }
        }


       }



    // Update is called once per frame
    void Update()
    {
        
    }

    
}
