using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class HoverMaterialScript : NetworkBehaviour
{
    public Material defaultMat;
    public Material selectedMat;

    public void Select()
    {
        GetComponent<Renderer>().material = selectedMat;
    }

    public void DeSelect()
    {
        GetComponent<Renderer>().material = defaultMat;
    }
}
