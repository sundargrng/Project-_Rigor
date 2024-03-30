using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour, IDataPersistence
{
    public Image actorImage;
    public Text actorName;
    public Text messageText;
    public RectTransform backgroundBox;
    public GameObject blackImage; // Reference to the black image GameObject
    public GameObject npcObject; // Reference to the NPC GameObject

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    public static bool isActive = false;

    private Rigidbody2D playerRB;
    private Animator playerAnim;

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        playerRB.velocity = Vector3.zero;
        playerAnim.SetFloat("moveX", 0);
        playerAnim.SetFloat("moveY", 0);

        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;

        Debug.Log("Started Conversation, Loaded Messages:" + messages.Length);
        DisplayMessage();
        backgroundBox.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    void DisplayMessage()
    {
        Message messageToDisplay = currentMessages[activeMessage];
        messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorID];
        actorName.text = actorToDisplay.actorName;
        actorImage.sprite = actorToDisplay.sprite;

        AnimateTextColor();
    }

    public void NextMessage()
    {
        activeMessage++;
        if (activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        }
        else
        {
            Debug.Log("Conversation Ended!");
            backgroundBox.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
            StartCoroutine(FadeAndDisableNPC());
        }
    }

    IEnumerator FadeAndDisableNPC()
    {
        blackImage.SetActive(true); // Activate the black image
        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds
        blackImage.SetActive(false); // Deactivate the black image
        isActive = false; // Disable player's movement


        // SOme NPCs in the game blocks the way and after we finish dialogue with that NPC, that NPC gameobject is disabled
        // Disable the NPC GameObject
        npcObject.SetActive(false);
    }

    void AnimateTextColor()
    {
        LeanTween.textAlpha(messageText.rectTransform, 0, 0);
        LeanTween.textAlpha(messageText.rectTransform, 1, 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        backgroundBox.transform.localScale = Vector3.zero;

        GameObject rb = GameObject.FindGameObjectWithTag("Player");
        if (rb != null)
        {
            playerRB = rb.GetComponent<Rigidbody2D>();
            playerAnim = rb.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive == true)
        {
            NextMessage();
        }
    }

    public void LoadData(GameData data)
    {
        // Load NPC state from saved data
        npcObject.SetActive(!data.isNPCDisabled);
    }

    public void SaveData(ref GameData data)
    {
        // Save NPC state to data
        data.isNPCDisabled = !npcObject.activeSelf;
    }
}
