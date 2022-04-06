using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{
    [SerializeField] GameObject VRcamera;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeCamera());
    }

    IEnumerator ChangeCamera()
    {
        yield return new WaitForSecondsRealtime(15);
        VRcamera.SetActive(false);
    }
    
}
