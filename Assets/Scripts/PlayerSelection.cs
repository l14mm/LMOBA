using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {

    private NetworkedPlayerScript currentlySelected = null;
    private BaseScript baseSelected = null;

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

            if (hit.transform.GetComponent<BaseScript>())
            {
                baseSelected = hit.transform.GetComponent<BaseScript>();
                baseSelected.Select();
            }
            else if (baseSelected)
            {
                baseSelected.DeSelect();
                baseSelected = null;
            }
        }
    }
}
