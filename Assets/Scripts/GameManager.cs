using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour {

    public GameObject p_AIPlayer;
    public List<NetworkStartPosition> spawns;
    public int numAI = 3;

    public List<string> playerList = new List<string>();

    void Start()
    {
        for(int i = 1; i <= numAI; i++)
        {
            GameObject aiPlayer = Instantiate(p_AIPlayer, spawns[i].transform.position, spawns[i].transform.rotation);            
            NetworkServer.Spawn(aiPlayer);
            if(i % 2 == 0)
                aiPlayer.GetComponent<NetworkedPlayerScript>().SetTeam(1);
            else
                aiPlayer.GetComponent<NetworkedPlayerScript>().SetTeam(2);
        }
    }
}
