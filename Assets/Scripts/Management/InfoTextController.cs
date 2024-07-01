//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTextController : MonoBehaviour
{
    void Update()
    {
        //Get direction by calculating positions of camera and this
        Vector3 v = Camera.main.transform.position - transform.position;
        //only turn around y-axis
        v.x = v.z = 0.0f;
        //Look at camera - v
        transform.LookAt(Camera.main.transform.position - v);
        //set this rotation to camera's rotation
        transform.rotation = (Camera.main.transform.rotation);
    }
}
