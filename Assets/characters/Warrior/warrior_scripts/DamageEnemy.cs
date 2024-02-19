using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    public int damageTaken = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            EnemyHealthManager eHealthMan;
            eHealthMan = other.gameObject.GetComponent<EnemyHealthManager>();
            eHealthMan.damageEnemy(damageTaken);
        }

        if (other.tag == "arrow")
        {
            Destroy(other.gameObject);
        }
    }


    
}
