using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class WarriorController : MonoBehaviour, IDataPersistence
{
    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField]
    private float speed;

    private float attackTime = 0.5f;
    private float attackCountdown;
    private bool isAttacking;

    public GameObject AttackPoint;
    public float AttackPointRadius;
    public LayerMask enemies;
    public int playerDamage;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync("Main Menu");
        }

        if (DialogManager.isActive == true)
        {
            return;
        }

        if (FlyingSlash.lemmeSlash == true)
        {
            // If LeftShift is pressed, prevent the player from moving
            rb.velocity = Vector2.zero;
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            return; // Exit the Update method
        }

        if (AreaTransitions.inputDisable == true)
        {
            return;
        }

        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * speed * Time.fixedDeltaTime;

        animator.SetFloat("moveX", rb.velocity.x);
        animator.SetFloat("moveY", rb.velocity.y);


        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            attackCountdown -= Time.deltaTime;
            if (attackCountdown < 0)
            {
                animator.SetBool("isAttacking", false);
                isAttacking = false;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private IEnumerator AttackWithDelay()
    {
        // Wait for the specified delay before attacking
        yield return new WaitForSeconds(0.4f);

        // Get the direction the player is facing
        Vector2 attackDirection = new Vector2(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY")).normalized;

        // Calculate attack point position based on direction
        Vector2 attackPosition = (Vector2)transform.position + attackDirection * AttackPointRadius;

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPosition, AttackPointRadius, enemies);

        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("Enemy is Hit");
            enemy.GetComponent<EnemyHealthManager>().TakeDamage(playerDamage);
        }
    }

    private void Attack()
    {
        attackCountdown = attackTime;
        animator.SetBool("isAttacking", true);
        isAttacking = true;

        // Start the coroutine for attacking with a delay
        StartCoroutine(AttackWithDelay());
    }


    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "arrow")
        {
            Destroy(other.gameObject);
        }
    }
}
