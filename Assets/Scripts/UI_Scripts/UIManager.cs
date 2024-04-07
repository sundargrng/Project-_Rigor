using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IDataPersistence
{
    private HealthManager healthManager;
    private Character playerStat;

    public Slider hpAmount;
    public Slider expAmount;

    public Text hpText;
    public Text expText;
    public Text currentLevel;
    public Text deathCountText; // Text element to display death count
    private int deathCount = 0; // Variable to store death count

    public Text keyCountText; // Text element to display key count
    private int keyCount = 0; // Variable to store key count

    private SaveSlot saveSlot;

    public Text roundText;
    public Text completionText;

    private Coroutine roundTextCoroutine;
    private Coroutine completionTextCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        healthManager = FindObjectOfType<HealthManager>();
        playerStat = FindObjectOfType<Character>();
        keyCountText.gameObject.SetActive(false); // Initially hide the key count UI

        // Find RoundBasedWaveSpawner in the scene
        RoundBasedWaveSpawner waveSpawner = FindObjectOfType<RoundBasedWaveSpawner>();

        if (waveSpawner != null)
        {
            // Subscribe to the OnRoundStart event
            waveSpawner.OnRoundStart.AddListener(OnRoundStart);
            waveSpawner.OnAllRoundsCompleted.AddListener(OnAllRoundsCompleted);
        }

        // Hide the completionText at start
        completionText.gameObject.SetActive(false);
    }

    void OnRoundStart(int roundNumber)
    {
        if (roundTextCoroutine != null)
        {
            StopCoroutine(roundTextCoroutine);
        }

        // Update round text to display the current round number
        roundText.text = "Round: " + roundNumber;
        roundText.gameObject.SetActive(true);

        // Start coroutine to fade out the round text after a short delay
        roundTextCoroutine = StartCoroutine(FadeOutRoundText());
    }

    IEnumerator FadeOutRoundText()
    {
        // Wait for 1 second before fading out
        yield return new WaitForSeconds(1f);

        // Fade out the round text over time
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            Color textColor = roundText.color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            roundText.color = textColor;
            timer += Time.deltaTime;
            yield return null;
        }

        roundText.gameObject.SetActive(false); // Deactivate round text after fading out
    }

    private void OnAllRoundsCompleted()
    {
        if (completionTextCoroutine != null)
        {
            StopCoroutine(completionTextCoroutine);
        }

        // Display the completion text
        completionText.text = "All Rounds Completed";
        completionText.gameObject.SetActive(true);

        completionTextCoroutine = StartCoroutine(FadeOutRoundCompletion());
    }

    IEnumerator FadeOutRoundCompletion()
    {
        // Wait for 1 second before fading out
        yield return new WaitForSeconds(3f);

        // Fade out the round text over time
        float fadeDuration = 1f;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            Color textColor = roundText.color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            roundText.color = textColor;
            timer += Time.deltaTime;
            yield return null;
        }

        completionText.gameObject.SetActive(false); // Deactivate round text after fading out
    }

    // Update is called once per frame
    void Update()
    {
        hpAmount.maxValue = healthManager.maxHealth;
        hpAmount.value = healthManager.currentHealth;

        expAmount.maxValue = playerStat.maxExperience;
        expAmount.value = playerStat.currentExperience;
        // Update health bar and exp bar text
        hpText.text = "HP " + healthManager.currentHealth + " / " + healthManager.maxHealth;
        expText.text = "EXP " + playerStat.currentExperience + " / " + playerStat.maxExperience;

        // Update current level text
        currentLevel.text = "Level " + playerStat.currentLevel;

        // Update death count text
        deathCountText.text = "Deaths: " + deathCount;

        // Check if the current scene is "Scene2"
        if (SceneManager.GetActiveScene().name == "level1")
        {
            // Show the key count UI
            keyCountText.gameObject.SetActive(true);

            // Update key count text
            keyCountText.text = "Keys: " + keyCount;
        }
        else
        {
            // Hide the key count UI if not in "Scene2"
            keyCountText.gameObject.SetActive(false);
        }
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
    }

    public void SaveData (GameData data)
    {
        data.deathCount = this.deathCount;
    }


    // Method to increment death count
    public void IncrementDeathCount()
    {
        deathCount++;
    }

    public void IncrementKeyCount()
    {
        keyCount++;
    }
}
