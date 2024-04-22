using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public int healthRestoreAmount; // New property to store health restoration amount
    public int expGainAmount;
    public int healthMinusAmount;

    public Loot(string lootName, int dropChance, int healthRestoreAmount, int expGainAmount, int healthMinusAmount)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
        this.healthRestoreAmount = healthRestoreAmount;
        this.expGainAmount = expGainAmount;
        this.healthMinusAmount = healthMinusAmount;
    }
}
