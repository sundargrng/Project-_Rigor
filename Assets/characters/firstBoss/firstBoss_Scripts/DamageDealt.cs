using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageDealt : MonoBehaviour
{
    private HealthManager healthManager;
    //private float loadingTime = 2f;
    //private bool gameOver;

    public float waitToDamage = 2f;
    public bool isDamaging;

    [SerializeField]
    private int damageDiff; // damage dealt by different enemies differs

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameOver)
        {
            loadingTime -= Time.deltaTime;

            if (loadingTime <= 0 )
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }*/


        /*if(isDamaging)
        {
            waitToDamage -= Time.deltaTime;
            if(waitToDamage <= 0 ) 
            {
                healthManager.damagePlayer(damageDiff);
                waitToDamage = 2f;
            }
        }*/
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

    /*private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isDamaging = false;
        }
    }*/
}
