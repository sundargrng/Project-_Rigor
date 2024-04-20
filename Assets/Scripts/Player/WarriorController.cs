using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using UnityEngine.UI;

public class WarriorController : MonoBehaviour, IDataPersistence
{
    public Image interactIcon;

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

    private Vector2 boxSize = new Vector2(0.1f, 1f);

    [SerializeField] private SpawnControlManager spawnManager;

    public float knockbackForce = 200f;

    // Reference to the PauseMenuScript (assigned in the Unity Editor)
    [SerializeField] private PauseMenuScript pauseMenuScript;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Find and assign interactable GameObject
        GameObject interactable = GameObject.FindGameObjectWithTag("Interact");
        if (interactable != null)
        {
            // Get the Image component from the interactable GameObject
            interactIcon = interactable.GetComponent<Image>();

            // Deactivate the interactIcon
            interactIcon.gameObject.SetActive(false);
        }

        // Find and assign SpawnControlManager if not already assigned
        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnControlManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseInput();

        if (FlyingSlash.lemmeSlash == true)
        {
            // If LeftShift is pressed, prevent the player from moving
            rb.velocity = Vector2.zero;
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            return; // Exit the Update method
        }

        if (!isPaused && !DialogManager.isActive && !AreaTransitions.inputDisable)
        {
            HandleMovement();
            HandleInteraction();
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

    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                MainMenu();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Settings();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                ResumeGame();
            }
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        Debug.Log("Game paused");
        isPaused = true;

        if (pauseMenuScript != null)
        {
            pauseMenuScript.Pause();
        }
    }

    private void ResumeGame()
    {
        Debug.Log("Game resumed");
        isPaused = false;

        if (pauseMenuScript != null)
        {
            pauseMenuScript.Resume();
        }
    }

    private void HandleMovement()
    {
        Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = movement * speed * Time.fixedDeltaTime;
        animator.SetFloat("moveX", rb.velocity.x);
        animator.SetFloat("moveY", rb.velocity.y);

        if (movement.magnitude > 0)
        {
            animator.SetFloat("lastMoveX", movement.x);
            animator.SetFloat("lastMoveY", movement.y);
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F) && spawnManager != null)
        {
            spawnManager.StartEnemySpawning();
            CheckInteraction();
        }
    }

    private IEnumerator AttackWithDelay()
    {
        yield return new WaitForSeconds(0.4f);
        Vector2 attackDirection = new Vector2(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY")).normalized;
        Vector2 attackPosition = (Vector2)transform.position + attackDirection * AttackPointRadius;

        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPosition, AttackPointRadius, enemies);

        foreach (Collider2D enemy in enemiesHit)
        {
            if (enemy != null)
            {
                EnemyHealthManager enemyHealth = enemy.GetComponent<EnemyHealthManager>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(playerDamage);
                }
                else
                {
                    Debug.Log("Enemy does not have EnemyHealthManager component.");
                }
            }
            else
            {
                Debug.Log("Enemy collider is null.");
            }
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

    public void OpenInteractableIcon()
    {
        if (interactIcon != null)
        {
            interactIcon.gameObject.SetActive(true);
        }
    }


    public void CloseInteractableIcon()
    {
        if (interactIcon != null)
        {
            interactIcon.gameObject.SetActive(false);
        }
    }


    private void CheckInteraction()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

        if (hits.Length > 0 )
        {
            foreach (RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }

    private void MainMenu()
    {
        /*if (isPaused)
        {
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadScene("Main Menu");
            ResumeGame(); // Ensure game resumes after returning to main menu
        }*/
        isPaused = false;

        if (pauseMenuScript != null)
        {
            pauseMenuScript.MainMenu();
        }
    }

    private void Settings()
    {
        isPaused = false;

        if (pauseMenuScript != null)
        {
            pauseMenuScript.Settings();
        }
    }
}
