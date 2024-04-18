using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingSlash : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 currentMousePos;

    public GameObject swordSlash;
    public Transform flyingSlashTransform;

    public bool canSlash = true;
    public float Timer;
    public float timeBetweenSlash;

    public Animator animator;

    public static bool lemmeSlash = false;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the "Player" tag and get its Animator component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            animator = player.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Could not find GameObject with tag 'Player'");
        }

        // Find the GameObject with the "MainCamera" tag and get its Camera component
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCamera != null)
        {
            mainCam = mainCamera.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("Could not find GameObject with tag 'MainCamera'");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            lemmeSlash = true;
            
            Vector3 currentMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = currentMousePos - transform.position;
            float rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotateZ);

            animator.SetFloat("stanceX", rotation.x);
            animator.SetFloat("stanceY", rotation.y);
            animator.SetBool("inStance", true);

            if (!canSlash)
            {
                Timer += Time.deltaTime;
                if (Timer > timeBetweenSlash)
                {
                    canSlash = true;
                    animator.SetFloat("slashX", 0);
                    animator.SetFloat("slashY", 0);
                    animator.SetBool("isSlashing", false);

                    Timer = 0;
                    Instantiate(swordSlash, flyingSlashTransform.position, Quaternion.identity);
                    SoundManager.PlaySound(SoundType.FLYINGSLASH);
                }
            }

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) && canSlash)
            {
                canSlash = false;
                animator.SetFloat("slashX", rotation.x);
                animator.SetFloat("slashY", rotation.y);
                animator.SetBool("isSlashing", true);
            }
        }
        else
        {
            lemmeSlash = false;

            animator.SetBool("inStance", false);
            animator.SetFloat("stanceX", 0); // Reset blend tree values to idle
            animator.SetFloat("stanceY", 0); // Reset blend tree values to idle

            animator.SetFloat("slashX", 0);
            animator.SetFloat("slashY", 0);
            animator.SetBool("isSlashing", false);
        }
    }
}
