using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuScript : MonoBehaviour {

    public NetworkedPlayerScript player;
    
    void Awake()
    {
        if (GetComponent<Canvas>().enabled)
            GetComponent<Canvas>().enabled = false;

        
    }

    public void BuyItem(Item item)
    {
        if(player.money >= 20 && player.GetComponent<InventoryManager>().inventoryCount < 6)
        {
            player.GetComponent<InventoryManager>().AddItem(item);
            player.money -= 20;
        }
    }
}
