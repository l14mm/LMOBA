using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class FancyScript : NetworkBehaviour
{

    public float speed = 5;
    public float damage = 20;
    [HideInInspector]
    public NetworkedPlayerScript creator;
    public GameObject trail;
    public GameObject sparks;
    public GameObject hitEffect;

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        if (other.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId == creator.netId)
        {
            //Debug.Log("auto hit 1st");
        }
        else if (other.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId != creator.netId)
        {
            OnHit();
            other.GetComponent<NetworkedPlayerScript>().RpcResolveHit(damage);
            //Debug.Log("auto hit enemy");
            //Destroy(gameObject);
        }
        else if (other.tag == "Tower")
        {
            OnHit();
            //Debug.Log("auto hit tower");
            other.GetComponent<TowerScript>().RpcResolveHit(damage);
            //Destroy(gameObject);
        }
        else if (other.tag == "Base")
        {
            OnHit();
            other.GetComponent<BaseScript>().RpcResolveHit(damage);
            //Debug.Log("auto hit enemy");
            //Destroy(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    void OnHit()
    {
        speed = 0;
        trail.SetActive(false);
        sparks.SetActive(false);
        hitEffect.SetActive(true);
        Invoke("Destroy", 1.0f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
