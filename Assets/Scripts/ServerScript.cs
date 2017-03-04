using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerScript : NetworkBehaviour {

    public int playerCount;
    public string names;

    public SyncListString namesList;

    void Awake ()
    {
        //players = new SyncListString();

        //if (!isServer)
        //return;

        namesList = new SyncListString();
	}
	

	void Update ()
    {

	}

    [ClientRpc]
    public void RpcUpdateCount(string name)
    {
        playerCount++;
        //Debug.Log("network name: " + name);
        names = name;
        namesList.Add(name);
        Debug.Log("list: " + namesList[0].ToString());

        GetComponent<NetworkedPlayerScript>().playerCount = playerCount;
        //Debug.Log("network names: " + names);
        GetComponent<NetworkedPlayerScript>().names = names;
        GetComponent<NetworkedPlayerScript>().namesList = namesList;
    }


}
