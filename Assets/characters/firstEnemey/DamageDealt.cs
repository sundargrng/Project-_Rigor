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
    public bool isAttacking;

    [SerializeField]
    private int damageDiff; // damage dealt by different enemies differs

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindAnyObjectByType<HealthManager>();
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


        if(isAttacking)
        {
            waitToDamage -= Time.deltaTime;
            if(waitToDamage <= 0 ) 
            {
                healthManager.damagePlayer(damageDiff);
                waitToDamage = 2f;
            }
        }
    }


    // as soon as we collide with the enemy
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            //other.gameObject.SetActive(false);

            other.gameObject.GetComponent<HealthManager>().damagePlayer(damageDiff);
            //gameOver = true;
        }
    }


    // as long as the enemy is attacking us
    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            isAttacking = true;
        }
    }


    // when no longer colliding with enemy or enemy no longer attacking us
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            isAttacking = false;
            waitToDamage = 2f;
        }
    }
}
