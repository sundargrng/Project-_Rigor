using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageDealt : MonoBehaviour
{

    private float loadingTime = 2f;
    private bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            loadingTime -= Time.deltaTime;

            if (loadingTime <= 0 )
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            other.gameObject.SetActive(false);
            gameOver = true;
        }
    }
}
