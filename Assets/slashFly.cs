using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashFly : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public int slashDamage;

    private EnemyHealthManager enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        GameObject enemyKoHealth = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyKoHealth != null)
        {
            enemyHealth = enemyKoHealth.GetComponent<EnemyHealthManager>();
        }


        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        rb.velocity = new Vector3(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x)* Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot+180);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyHealth.GetComponent<EnemyHealthManager>().TakeDamage(slashDamage);

        }
    }
}
