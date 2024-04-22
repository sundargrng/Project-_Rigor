using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaTransitions : MonoBehaviour
{
    private Camera cam;
    private CameraController camController;
    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public Vector3 movePlayer;

    public Image fadeImage;
    public float newCamSize;

    private bool inTransition = false;
    public static bool inputDisable = false;
    public Rigidbody2D pRB;

    void Start()
    {
        cam = Camera.main;
        camController = Camera.main.GetComponent<CameraController>();
        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            pRB = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Could not find GameObject with tag 'Player'");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !inTransition)
        {
            inTransition = true;
            inputDisable = true;
            pRB.velocity = Vector3.zero;

            // Show black screen
            fadeImage.color = new Color(0, 0, 0, 1f);
            LeanTween.alpha(fadeImage.rectTransform, 1f, 1f);

            // Perform camera transition
            camController.minPosition = newCamMinPos;
            camController.maxPosition = newCamMaxPos;
            cam.orthographicSize = newCamSize;

            // Move player to new area
            other.transform.position += movePlayer;
            camController.target = other.transform;

            // Fade out black screen after camera transition
            StartCoroutine(FadeOutBlackScreen());
        }
    }

    private IEnumerator FadeOutBlackScreen()
    {
        yield return new WaitForSeconds(2f);

        // Fade out the black screen
        fadeImage.color = new Color(0, 0, 0, 0);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 0.5f);

        // Reset transition state
        inTransition = false;
        inputDisable = false;
    }
}
