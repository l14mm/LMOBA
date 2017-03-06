using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabCanvasScript : MonoBehaviour {

    public List<Text> playerList = new List<Text>();
    private GameManager gManager;
    
    void Awake()
    {
        gManager = GameObject.Find("Server").GetComponent<GameManager>();
    }

    void Update ()
    {
		for(int i = 0; i < playerList.Count; i++)
        {
            if(i < gManager.playerList.Count)
            {
                playerList[i].GetComponent<Text>().text = gManager.playerList[i];
            }
        }
	}
}
