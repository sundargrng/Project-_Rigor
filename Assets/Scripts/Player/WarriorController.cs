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

    public float knockbackForce = 200f;

    // Reference to the PauseMenuScript (assigned in the Unity Editor)
    [SerializeField] private PauseMenuScript pauseMenuScript;

    private bool isPaused = false;

    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    private bool isDashing;
    private bool canDash = true;

    // Start is called before the first frame update
    void Start()
    {
        canDash = true;

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

    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        HandlePauseInput();

        /*if (FlyingSlash.lemmeSlash == true && DialogManager.isActive == true)
        {
            // If LeftShift is pressed, prevent the player from moving
            rb.velocity = Vector2.zero;
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", 0);
            return; // Exit the Update method
        }*/

        if (!DialogManager.isActive && !AreaTransitions.inputDisable)
        {
            if (FlyingSlash.lemmeSlash == true)
            {
                // If LeftShift is pressed, prevent the player from moving
                rb.velocity = Vector2.zero;
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 0);
                return; // Exit the Update method
            }

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
            if (DialogManager.isActive == true)
            {
                return; // Exit the Update method
            }

            Attack();
        }

        // Check for dash input and cooldown status
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.C)) && canDash)
        {
            StartCoroutine(HandleDash());
        }
    }


    private void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
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
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
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

    private void CloseMenu()
    {
        Debug.Log("Pause Menu Closed");
        isPaused = false;

        if (pauseMenuScript != null)
        {
            pauseMenuScript.Resume();
        }
    }

    public void ReturnPauseMenu()
    {
        isPaused = true;
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

    private IEnumerator HandleDash()
    {
        if (isDashing || !canDash)
            yield break;

        isDashing = true;
        canDash = false;

        // Determine dash direction based on last movement
        Vector2 dashDir = new Vector2(animator.GetFloat("lastMoveX"), animator.GetFloat("lastMoveY")).normalized;

        // If no prior movement, dash in the current facing direction
        if (dashDir.magnitude == 0)
        {
            dashDir = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY")).normalized;
        }
        tr.emitting = true;

        // Store original color and reduce alpha during dash
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color dashColor = originalColor;
        dashColor.a = 0.5f; // Set alpha to 0.5 (50% transparency) during dash
        spriteRenderer.color = dashColor;

        // Set dashX and dashY parameters for dash animation
        animator.SetBool("dashing", true);
        animator.SetFloat("dashX", dashDir.x);
        animator.SetFloat("dashY", dashDir.y);

        // Dash duration timer
        float dashTimer = 0f;

        bool hasDamagedEnemies = false; // Flag to track if enemies have been damaged during this dash

        while (dashTimer < dashDuration)
        {
            Vector2 dashPosition = rb.position + dashDir * dashSpeed * Time.deltaTime;

            // Perform overlap detection to find enemies hit during dash
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(dashPosition, AttackPointRadius, enemies);

            foreach (Collider2D enemyCollider in hitEnemies)
            {
                EnemyHealthManager enemyHealth = enemyCollider.GetComponent<EnemyHealthManager>();
                if (enemyHealth != null && !hasDamagedEnemies)
                {
                    enemyHealth.TakeDamage(playerDamage);
                    hasDamagedEnemies = true; // Set flag to true once damage is applied
                }
            }

            dashTimer += Time.deltaTime;
            rb.velocity = dashDir * dashSpeed;
            yield return null;
        }

        // Reset movement animation parameters to dash direction
        animator.SetBool("dashing", false);
        animator.SetFloat("moveX", dashDir.x);
        animator.SetFloat("moveY", dashDir.y);
        isDashing = false;
        tr.emitting = false;

        // Restore original color after dash
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
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
