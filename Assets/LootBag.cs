using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();
    public float dropForce = 300f;

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); //0 and 101 excluded
        List<Loot> possibleItems = new List<Loot>();

        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }

        Debug.Log("No Loots");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();

        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;

            // Apply an impulse force in a random direction
            Vector2 dropDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Rigidbody2D rb = lootGameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(dropDirection * dropForce, ForceMode2D.Impulse);

            // Stop the object after applying force
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
