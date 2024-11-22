using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Openable : Interactable, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool opened = false;

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
                opened = true;
                lootBag.SpawnLoot(transform.position);
                this.gameObject.SetActive(false);
            }
        }
    }

    private void SetClosed()
    {
        spriteRenderer.sprite = closedSprite;
        isOpen = false;
    }

    public void LoadData(GameData data)
    {
        data.chestOpened.TryGetValue(id, out opened);
        if (opened)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.chestOpened.ContainsKey(id))
        {
            data.chestOpened.Remove(id);
        }
        data.chestOpened.Add(id, opened);
    }
}
