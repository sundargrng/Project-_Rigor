using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxhealth;

    private bool hurtFlash;

    [SerializeField]
    private float flashTime = 0f;
    private float flashCountDown = 0f;

    private SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hurtFlash)
        {
            if (flashCountDown > flashTime * .99f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCountDown > flashTime * .75f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCountDown > flashTime * .50f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else if (flashCountDown > flashTime * .25f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
            }
            else if (flashCountDown > 0f)
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 0f);
            }
            else
            {
                enemySprite.color = new Color(enemySprite.color.r, enemySprite.color.g, enemySprite.color.b, 1f);
                hurtFlash = false;
            }
            flashCountDown -= Time.deltaTime;
        }
    }

    public void damageEnemy(int damageTaken)
    {
        currentHealth -= damageTaken;
        hurtFlash = true;
        flashCountDown = flashTime;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
