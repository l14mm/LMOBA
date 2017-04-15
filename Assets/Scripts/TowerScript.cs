using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TowerScript : NetworkBehaviour {

    public Transform shootingPosition;
    public NetworkedPlayerScript target;
    private LineRenderer lineRenderer;
    public Light light;
    public float range = 10;

    public Transform healthBar;
    [SyncVar]
    public float maxHealth;
    [SyncVar]
    public float healthRegen;
    [SyncVar]
    public float health;
    [SyncVar]
    public float armour;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, shootingPosition.position);
    }

    private void Start()
    {
        maxHealth = 200;
        healthRegen = 0.1f;
        health = 100;
        armour = 250;
    }

    private void FixedUpdate()
    {
        if (health < maxHealth)
            health += healthRegen;
    }

    private void Update()
    {
        if (health > maxHealth) health = maxHealth;
        // Healthbar
        healthBar.transform.forward = Vector3.up;
        //healthBar.transform.position = transform.position + Vector3.forward;
        healthBar.localScale = new Vector3(((health * 100) / maxHealth) * 0.005f, 0.2f, 1);

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

    [ClientRpc]
    public void RpcResolveHit(float damage)
    {
        health -= (damage * (1 - (armour / 1000)));
        //Debug.Log("tower took " + (damage * (1 - (armour / 1000))) + " damage");
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
