using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public int currentHealth;
    public int maxhealth;

    private float flashDuration = 0.1f;
    private float flashCountDown = 0f;

    private SpriteRenderer enemySprite;

    private bool isFlashing;

    // Delay variables
    private float moveDelay = 0.15f; // The duration of the delay in seconds

    private Animator animator;

    [SerializeField]private Rigidbody2D rb2d;

    [SerializeField]private float strength;

    private Transform sender;

    [SerializeField] private Transform receiver;

    public static bool isDying = false;

    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sender = FindObjectOfType<WarriorController>().transform;
        receiver = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            FlashRed();
        }
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isFlashing = true;
        flashCountDown = flashDuration;
        
        Vector2 distance = (receiver.transform.position - sender.transform.position).normalized;
        rb2d.AddForce(distance*strength, ForceMode2D.Impulse);

        StartCoroutine(MoveDelay());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(moveDelay); // Wait for the specified delay
        rb2d.velocity = Vector2.zero;
    }

    public void FlashRed()
    {
        if (flashCountDown > 0f)
        {
            enemySprite.color = new Color(1f, 0f, 0f, 1f); // Set the sprite color to red
            flashCountDown -= Time.deltaTime;
        }
        else
        {
            enemySprite.color = new Color(1f, 1f, 1f, 1f); // Set the sprite color back to white
            isFlashing = false;
        }
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        StartCoroutine(DisableObjectAfterAnimation());
        isDying = true;
    }

    IEnumerator DisableObjectAfterAnimation()
    {
        // Fade out the sprite using LeanTween
        LeanTween.alpha(gameObject, 0f, 1.0f);

        // Set the object inactive after the fade is complete
        yield return new WaitForSeconds(1.0f); // Wait for the fade duration
        gameObject.SetActive(false);
    }
}
