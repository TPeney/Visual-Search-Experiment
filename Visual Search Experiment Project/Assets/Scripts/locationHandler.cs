using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class locationHandler : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] cueLocations;

    public GameObject[] createCueLocationArray()
    {
        // Create list of Children GameObjects - Each cue location
        cueLocations = new GameObject[transform.childCount];
        int i = 0;
        foreach (Transform child in transform)
        {
            cueLocations[i] = child.gameObject;
            i += 1;
        }

        // Shuffle locations
        for (int t = 0; t < cueLocations.Length; t++)
        {
            GameObject tmp = cueLocations[t];
            int r = Random.Range(t, cueLocations.Length);
            cueLocations[t] = cueLocations[r];
            cueLocations[r] = tmp;
        }

        return cueLocations;
    }
}
