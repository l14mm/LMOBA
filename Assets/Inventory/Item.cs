using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    public enum ItemType
    {
        sword, shield, healthPotion, manaPotion, boots,
    }

    public float stat_AttackDamage = 0;
    public float stat_Armour = 0;
    public float stat_Speed = 0;

    public Sprite sprite;
    public ItemType type;

    public bool isConsumable = false;

    public void Consume(NetworkedPlayerScript player)
    {
        if (!isConsumable || !player)
            return;

        if(type == ItemType.healthPotion)
        {
            player.health += 20;
        }
        if (type == ItemType.manaPotion)
        {
            player.mana += 20;
        }
    }
}
