using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackScript : NetworkBehaviour
{

    private float speed = 40;
    [HideInInspector]
    public NetworkedPlayerScript creator;
    public Transform target;

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if(target)
        {
            transform.LookAt(target);
            //transform.position = Vector3.Lerp(transform.position, target.position, 0.01f);
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent && other.transform.parent.tag == "Player" && other.GetComponentInParent<NetworkedPlayerScript>().netId == creator.netId)
        {
            //Debug.Log("auto hit 1st");
        }
        else if (other.transform.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId == creator.netId)
        {
            //Debug.Log("auto hit same");
        }
        else if (other.transform.parent && other.transform.parent.tag == "Player" && other.GetComponentInParent<NetworkedPlayerScript>().netId != creator.netId)
        {
            other.GetComponentInParent<NetworkedPlayerScript>().RpcResolveHit(10);
            //Debug.Log("auto hit enemy");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
