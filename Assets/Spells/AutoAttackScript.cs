using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackScript : NetworkBehaviour
{

    public float speed = 5;
    public float damage = 10;
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
            transform.position = Vector3.MoveTowards(transform.position, target.position, 0.1f * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ( other.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId == creator.netId)
        {
            //Debug.Log("auto hit 1st");
        }
        else if (other.transform.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId == creator.netId)
        {
            //Debug.Log("auto hit same");
        }
        else if (other.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId != creator.netId)
        {
            other.GetComponent<NetworkedPlayerScript>().RpcResolveHit(damage);
            //Debug.Log("auto hit enemy");
            Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }
}
