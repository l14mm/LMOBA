using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {

	private void Update()
    {
        if(GetComponent<NetworkedPlayerScript>().myCamera)
        {
            GameObject fowCamera = GameObject.Find("FogOfWarCamera");
            fowCamera.transform.parent = GetComponent<NetworkedPlayerScript>().myCamera.transform;
            fowCamera.GetComponent<Camera>().orthographic = false;
        }
    }
}
