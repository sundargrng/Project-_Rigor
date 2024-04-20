using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public Transform destination; // teleport to another portal

    private Camera cam;
    private CameraController camController;
    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public float newCamSize;

    public Image fadeImage;

    private bool inTransition = false;

    public static bool inputDisable = false;

    public Rigidbody2D pRB;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camController = Camera.main.GetComponent<CameraController>();

        // Find the GameObject with the "Player" tag and get its Animator component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            pRB = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Could not find GameObject with tag 'Player'");
        }

        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !inTransition)
        {
            inTransition = true;

            inputDisable = true;
            pRB.velocity = Vector3.zero;
            //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
            // Show black screen
            fadeImage.color = new Color(0, 0, 0, 1f);
            LeanTween.alpha(fadeImage.rectTransform, 1f, 1f);

            // Perform camera transition
            camController.minPosition = newCamMinPos;
            camController.maxPosition = newCamMaxPos;

            cam.orthographicSize = newCamSize;

            camController.target = other.transform;

            other.transform.position = destination.transform.position;

            // Fade out black screen after camera transition
            StartCoroutine(FadeOutBlackScreen());
        }
    }

    IEnumerator FadeOutBlackScreen()
    {
        // Wait for camera transition and player movement to complete
        yield return new WaitForSeconds(2f);
        fadeImage.color = new Color(0, 0, 0, 0);
        LeanTween.alpha(fadeImage.rectTransform, 0f, 0.5f);
        inTransition = false;

        inputDisable = false;

    }
}
