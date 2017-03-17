using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour {



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void BuyItem(Item item)
    {
        GameObject.Find("LOCAL Player").GetComponent<InventoryManager>().AddItem(item);
        GameObject.Find("LOCAL Player").GetComponent<NetworkedPlayerScript>().money -= 20;
    }
}
