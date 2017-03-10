using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWPlaneEnable : MonoBehaviour {

	private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = true;
        Destroy(this);
    }
}
