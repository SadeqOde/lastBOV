using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [System.Serializable]
    public class Item
    {
        public string itemName;
        public Sprite itemIcon;
        public int itemCount;
        public int amount;
    }

    public Item[] items;

}
