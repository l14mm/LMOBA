using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {

    private float respawnTime = 2;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        { 
            NetworkedPlayerScript player = col.GetComponent<NetworkedPlayerScript>();
            if (player.health < player.maxHealth)
            {
                player.health += 20;
                //Destroy(gameObject);
                gameObject.SetActive(false);
                Invoke("Respawn", respawnTime);
            }
        }
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
    }
}
