using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName ="BuffEffect/HealthBuff")]
public class HealthBuff : BuffEffect
{
    public Sprite sprite;
    public int amount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<HealthManager>().currentHealth += amount;
    }
}
