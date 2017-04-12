using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {

    private HoverMaterialScript currentlySelected = null;

	void Update ()
    {
        RaycastHit hit;
        if (Physics.Raycast(GetComponent<NetworkedPlayerScript>().myCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.GetComponent<HoverMaterialScript>())
            {
                if (hit.transform.GetComponent<NetworkedPlayerScript>() &&
                    hit.transform.GetComponent<NetworkedPlayerScript>().team == GetComponent<NetworkedPlayerScript>().team)
                    return;
                currentlySelected = hit.transform.GetComponent<HoverMaterialScript>();
                currentlySelected.Select();
            }
            else if (currentlySelected)
            {
                currentlySelected.DeSelect();
                currentlySelected = null;
            }
        }
    }
}
