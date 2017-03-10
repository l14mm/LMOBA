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
            if (player.health < 100)
            {
                player.health += 20;
                if (player.health > 100)
                    player.health = 100;
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
