using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuScript : MonoBehaviour {

    public NetworkedPlayerScript player;

    public GameObject playerItem1;
    public GameObject playerItem2;
    public GameObject playerItem3;
    public GameObject playerItem4;
    public GameObject playerItem5;
    public GameObject playerItem6;

    void Awake()
    {
        if (GetComponent<Canvas>().enabled)
            GetComponent<Canvas>().enabled = false;

        playerItem1.GetComponent<Image>().enabled = false;
        playerItem2.GetComponent<Image>().enabled = false;
        playerItem3.GetComponent<Image>().enabled = false;
        playerItem4.GetComponent<Image>().enabled = false;
        playerItem5.GetComponent<Image>().enabled = false;
        playerItem6.GetComponent<Image>().enabled = false;
    }

    public void BuyItem(Item item)
    {
        if(player.money >= 20 && player.GetComponent<InventoryManager>().inventoryCount < 6)
        {
            player.GetComponent<InventoryManager>().AddItem(item);
            player.money -= 20;
        }
    }

    public void SellItem(int slot)
    {
        player.money += 20;
        player.GetComponent<InventoryManager>().RemoveItem(slot);
    }

    void Update()
    {
        if (player && GetComponent<Canvas>().enabled)
        {
            if (player.GetComponent<InventoryManager>().Item1)
            {
                playerItem1.GetComponent<Image>().enabled = true;
                playerItem1.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item1.GetComponent<Item>().sprite;
            }
            else
                playerItem1.GetComponent<Image>().enabled = false;
            if (player.GetComponent<InventoryManager>().Item2)
            {
                playerItem2.GetComponent<Image>().enabled = true;
                playerItem2.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item2.GetComponent<Item>().sprite;
            }
            else
                playerItem2.GetComponent<Image>().enabled = false;
            if (player.GetComponent<InventoryManager>().Item3)
            {
                playerItem3.GetComponent<Image>().enabled = true;
                playerItem3.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item3.GetComponent<Item>().sprite;
            }
            else
                playerItem3.GetComponent<Image>().enabled = false;
            if (player.GetComponent<InventoryManager>().Item4)
            {
                playerItem4.GetComponent<Image>().enabled = true;
                playerItem4.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item4.GetComponent<Item>().sprite;
            }
            else
                playerItem4.GetComponent<Image>().enabled = false;
            if (player.GetComponent<InventoryManager>().Item5)
            {
                playerItem5.GetComponent<Image>().enabled = true;
                playerItem5.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item5.GetComponent<Item>().sprite;
            }
            else
                playerItem5.GetComponent<Image>().enabled = false;
            if (player.GetComponent<InventoryManager>().Item6)
            {
                playerItem6.GetComponent<Image>().enabled = true;
                playerItem6.GetComponent<Image>().sprite = player.GetComponent<InventoryManager>().Item6.GetComponent<Item>().sprite;
            }
            else
                playerItem6.GetComponent<Image>().enabled = false;
        }
    }

    void OnGUI2()
    {
        if (player && GetComponent<Canvas>().enabled)
        {
            if(player.GetComponent<InventoryManager>().Item1)
            {
                GUI.DrawTexture(new Rect(new Vector2(400, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item1.GetComponent<Item>().sprite.texture);
            }
            if (player.GetComponent<InventoryManager>().Item2)
            {
                GUI.DrawTexture(new Rect(new Vector2(460, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item2.GetComponent<Item>().sprite.texture);
            }
            if (player.GetComponent<InventoryManager>().Item3)
            {
                GUI.DrawTexture(new Rect(new Vector2(520, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item3.GetComponent<Item>().sprite.texture);
            }
            if (player.GetComponent<InventoryManager>().Item4)
            {
                GUI.DrawTexture(new Rect(new Vector2(580, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item4.GetComponent<Item>().sprite.texture);
            }
            if (player.GetComponent<InventoryManager>().Item5)
            {
                GUI.DrawTexture(new Rect(new Vector2(640, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item5.GetComponent<Item>().sprite.texture);
            }
            if (player.GetComponent<InventoryManager>().Item6)
            {
                GUI.DrawTexture(new Rect(new Vector2(700, 300), new Vector2(50, 50)), player.GetComponent<InventoryManager>().Item6.GetComponent<Item>().sprite.texture);
            }
        }
    }
}
