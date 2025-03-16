using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;

    public Image imagePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    List<Item> list = new();
    // Update is called once per frame
    void Update()
    {
        if (inventory.Items.Count != list.Count)
        {
            UpdateInventory();
            list.Clear();
            list.AddRange(inventory.Items);
        }
    }

    void UpdateInventory()
    {
        for (int i =0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }


        foreach (Item i in inventory.Items)
        {
            Image image = Instantiate(imagePrefab, transform);
            image.sprite = i.pocketSprite;
        }
    }
}
