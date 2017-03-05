using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : NetworkBehaviour {

    private ParticleSystem ps;
    private float speed = 10;
    [HideInInspector]
    public NetworkedPlayerScript creator;
    public GameObject p_FireExplosion;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

    public void Fire()
    {
        //speed = 1;
        //ps.main.startSpeed = 30;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void Remove()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent && other.transform.parent.tag == "Player" && other.GetComponentInParent<NetworkedPlayerScript>().netId == creator.netId)
        {

        }
        else if (other.transform.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId == creator.netId)
        {

        }
        else if (other.transform.parent && other.transform.parent.tag == "Player" && other.GetComponentInParent<NetworkedPlayerScript>().netId != creator.netId)
        {
            other.transform.parent.GetComponent<NetworkedPlayerScript>().RpcResolveHit((int)(10 * (transform.localScale.x * 5)));
            Instantiate(p_FireExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player" && other.GetComponent<NetworkedPlayerScript>().netId != creator.netId)
        {
            other.gameObject.GetComponent<NetworkedPlayerScript>().RpcResolveHit((int)(10 * (transform.localScale.x * 5)));
            Instantiate(p_FireExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            //Debug.Log("hit: " + other.gameObject.name);
            //Destroy(other);
            Instantiate(p_FireExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
