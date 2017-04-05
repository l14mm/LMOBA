using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class HoverMaterialScript : NetworkBehaviour
{
    public Material defaultMat;
    public Material selectedMat;
    public List<Renderer> models = new List<Renderer>();

    public void Select()
    {
        //GetComponent<Renderer>().material = selectedMat;
        for(int i = 0; i < models.Count; i++)
        {
            models[i].material = selectedMat;
        }
    }

    public void DeSelect()
    {
        //GetComponent<Renderer>().material = defaultMat;
        for (int i = 0; i < models.Count; i++)
        {
            models[i].material = defaultMat;
        }
    }
}
