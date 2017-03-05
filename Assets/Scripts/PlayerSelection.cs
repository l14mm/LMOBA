using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {

    private NetworkedPlayerScript currentlySelected = null;

	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if(hit.transform.GetComponent<NetworkedPlayerScript>())
            {
                currentlySelected = hit.transform.GetComponent<NetworkedPlayerScript>();
                currentlySelected.Select();
            }
            else if(currentlySelected)
            {
                currentlySelected.DeSelect();
                currentlySelected = null;
            }
        }
    }
}
