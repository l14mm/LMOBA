using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : NetworkBehaviour {

    private ParticleSystem ps;
    private float speed = 0;
    [HideInInspector]
    public NetworkedPlayerScript creator;

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
        transform.Translate(Vector3.forward * Time.deltaTime * 10);
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
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<NetworkedPlayerScript>().netId.Value != creator.netId.Value)
        {
            other.gameObject.GetComponent<NetworkedPlayerScript>().RpcResolveHit((int)(10 * (transform.localScale.x * 5)));
            Destroy(gameObject);
        }
    }
}
