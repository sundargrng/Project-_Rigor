using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTypeSpike : MonoBehaviour
{
    public int damage = 5; // The amount of damage the spike trap deals
    public float checkInterval = 1f; // The interval between collision checks
    public float detectionRadius = 1f; // The radius of the detection area

    private void Start()
    {
        StartCoroutine(CheckForCollisions());
    }

    private IEnumerator CheckForCollisions()
    {
        while (true)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask.GetMask("Player"));
            foreach (Collider2D hit in hits)
            {
                HealthManager healthManager = hit.gameObject.GetComponent<HealthManager>();
                if (healthManager != null)

                {
                    healthManager.damagePlayer(damage);
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}