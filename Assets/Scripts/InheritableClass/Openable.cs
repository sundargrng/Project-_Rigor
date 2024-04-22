using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Openable : Interactable
{
    public Sprite openSprite;
    public Sprite closedSprite;

    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetClosed(); // Set the initial state to closed
    }

    public override void Interact()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        spriteRenderer.sprite = openSprite;
        isOpen = true;
        StartCoroutine(SpawnLootAfterDelay());
    }

    private void Close()
    {
        spriteRenderer.sprite = closedSprite;
        isOpen = false;
    }

    private IEnumerator SpawnLootAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (isOpen)
        {
            LootBag lootBag = GetComponent<LootBag>();
            if (lootBag != null)
            {
                lootBag.SpawnLoot(transform.position);
            }
        }
    }

    private void SetClosed()
    {
        spriteRenderer.sprite = closedSprite;
        isOpen = false;
    }
}
