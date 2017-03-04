using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour {

    public GameObject p_AIPlayer;
    public List<NetworkStartPosition> spawns;

    void Start()
    {
        int numAI = 2;
        for(int i = 1; i < numAI + 1; i++)
        {
            GameObject aiPlayer = Instantiate(p_AIPlayer, spawns[i].transform.position, spawns[i].transform.rotation);            
            NetworkServer.Spawn(aiPlayer);
        }
    }
}
