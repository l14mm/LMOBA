using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : MonoBehaviour {

    public Transform shootingPosition;
    public NetworkedPlayerScript target;
    private LineRenderer lineRenderer;
    public Light light;
    public float range = 10;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, shootingPosition.position);
    }

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            GameObject temp = hitColliders[i].gameObject;
            //if (temp.tag == "Player" && temp.GetComponent<NetworkedPlayerScript>().netId != netId && temp.GetComponent<NetworkedPlayerScript>().isAI)
            if (temp.tag == "Player")
            {
                target = temp.GetComponent<NetworkedPlayerScript>();
            }
        }
        if (target && Vector3.Distance(shootingPosition.position, target.transform.position) < range)
        {
            target.RpcResolveHit(0.5f);
            lineRenderer.enabled = true;
            light.enabled = true;
            lineRenderer.SetPosition(1, target.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
            light.enabled = false;
            target = null;
        }
    }
}
