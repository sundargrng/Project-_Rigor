using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

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

    [SerializeField] private GameObject damagePopUpPrefab;

    [SerializeField] private EnemyHpBar healthBar;

    public static bool deadFr = false;

    private bool defeated = false;

    [SerializeField] private int expAmount;

    // Reference to UIManager
    private UIManager uiManager;

    private TrapTypeChest trapTypeChest;


    // Start is called before the first frame update
    void Start()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sender = FindObjectOfType<WarriorController>().transform;
        receiver = GetComponent<Transform>();

        currentHealth = maxhealth;

        healthBar = FindObjectOfType<EnemyHpBar>();

        // Find UIManager in the scene
        uiManager = FindObjectOfType<UIManager>();

        trapTypeChest = GameObject.FindGameObjectWithTag("TrapChest").GetComponent<TrapTypeChest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            FlashRed();
        }
    }

    public void TakingSwordWaves(int damage)
    {
        if (currentHealth > 0 && !defeated)
        {
            SoundManager.PlaySound(SoundType.SLASHDAMAGE);
            ShowDamage(damage.ToString());
            currentHealth -= damage;
            isFlashing = true;
            flashCountDown = flashDuration;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        defeated = false;
        SoundManager.PlaySound(SoundType.SWORDDAMAGE);
        currentHealth -= damage;
        isFlashing = true;
        flashCountDown = flashDuration;
        
        Vector2 distance = (receiver.transform.position - sender.transform.position).normalized;
        rb2d.AddForce(distance*strength, ForceMode2D.Impulse);

        StartCoroutine(MoveDelay(damage));

        if (currentHealth <= 0)
        {
            // Disable physics interactions by setting Rigidbody2D's isKinematic to true
            rb2d.isKinematic = true;
            Die();
        }
    }

    IEnumerator MoveDelay(int damage)
    {
        yield return new WaitForSeconds(moveDelay); // Wait for the specified delay
        rb2d.velocity = Vector2.zero;

        if(rb2d.velocity == Vector2.zero)
        {
            ShowDamage(damage.ToString());
        }
    }

    void ShowDamage(string text)
    {
        if (damagePopUpPrefab)
        {
            GameObject prefab = Instantiate(damagePopUpPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().text = text;
        }
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

    public void Die()
    {
        trapTypeChest.OnEnemyKilled();
        //healthBar.gameObject.SetActive(false);
        GetComponent<LootBag>().SpawnLoot(transform.position);
        deadFr = true;

        defeated = true;
        SoundManager.PlaySound(SoundType.ENEMYDEATH);
        animator.SetTrigger("isDead");

        // Disable collider to prevent further interactions
        Collider2D enemyCollider = GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        StartCoroutine(DisableObjectAfterAnimation());
    }

    IEnumerator DisableObjectAfterAnimation()
    {
        // Fade out the sprite using LeanTween
        LeanTween.alpha(gameObject, 0f, 1.0f);

        // Set the object inactive after the fade is complete
        yield return new WaitForSeconds(1.0f); // Wait for the fade duration
        

        gameObject.SetActive(false);
        
        ExperienceManager.Instance.AddExperience(expAmount);

    }

    public void LoadData(GameData data)
    {
        data.enemiesDefeated.TryGetValue(id, out defeated);
        if (defeated)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if (data.enemiesDefeated.ContainsKey(id))
        {
            data.enemiesDefeated.Remove(id);
        }
        data.enemiesDefeated.Add(id, defeated);
    }
}
