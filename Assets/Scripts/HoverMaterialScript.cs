using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class HoverMaterialScript : NetworkBehaviour
{
    public Material defaultMat;
    public Material _selectedMat;
    public List<Renderer> models = new List<Renderer>();
    private Material mySelectedMat;
    public Color outlineColour = Color.red;
    [Range(0.0f, 1.0f)]
    public float outlineWidth = 0.33f;

    void Start()
    {
        //if (!_selectedMat)
            //return;

        // Make a new material out of the reference so we don't change the default selected material for everthing
        mySelectedMat = new Material(_selectedMat);

        mySelectedMat.SetTexture("_MainTex", defaultMat.mainTexture);
        mySelectedMat.SetColor("_OutlineColor", outlineColour);
        mySelectedMat.SetFloat("_Outline", outlineWidth);
        // = new Color(255, 0, 164)
        //Select();
    }

    void Update2()
    {
        mySelectedMat.SetColor("_OutlineColor", outlineColour);
        mySelectedMat.SetFloat("_Outline", outlineWidth);
    }

    public void Select()
    {
        //GetComponent<Renderer>().material = selectedMat;
        for(int i = 0; i < models.Count; i++)
        {
            models[i].material = mySelectedMat;
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
