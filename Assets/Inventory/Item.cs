using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        sword, shield
    }

    public Sprite sprite;
    public ItemType type;
    public float attack;
    public float armour;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
