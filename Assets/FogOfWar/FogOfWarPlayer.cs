﻿using UnityEngine;
using System.Collections;

public class FogOfWarPlayer : MonoBehaviour
{

    public Transform FogOfWarPlane;
    public int Number = 1;

    // Use this for initialization
    void Awake()
    {
        FogOfWarPlane = GameObject.Find("FogOfWarPlane").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (FogOfWarPlane)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Ray rayToPlayerPos = Camera.main.ScreenPointToRay(screenPos);
            int layermask = (int)(1 << 8);
            RaycastHit hit;
            if (Physics.Raycast(rayToPlayerPos, out hit, 1000, layermask))
            {
                FogOfWarPlane.GetComponent<Renderer>().material.SetVector("_Player" + Number.ToString() + "_Pos", hit.point);
                Debug.Log("plane");
            }
            FogOfWarPlane.GetComponent<Renderer>().material.SetVector("_Player" + Number.ToString() + "_Pos", transform.position);
        }
    }
}