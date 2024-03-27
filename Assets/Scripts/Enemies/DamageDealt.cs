using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageDealt : MonoBehaviour
{
    private HealthManager healthManager;
    
    [SerializeField]
    private int damageDiff; // damage dealt by different enemies differs

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Access the player's HealthManager and apply damage
            HealthManager playerHealth = other.GetComponent<HealthManager>();
            if (playerHealth != null)
            {
                playerHealth.damagePlayer(damageDiff);
            }
        }
    }
}
