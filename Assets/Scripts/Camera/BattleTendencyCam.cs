using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTendencyCam : MonoBehaviour
{
    private Camera cam;
    private CameraController camController;
    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public Vector3 movePlayer;

    public float newCamSize;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        camController = Camera.main.GetComponent<CameraController>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {


            // Perform camera transition
            camController.minPosition = newCamMinPos;
            camController.maxPosition = newCamMaxPos;

            cam.orthographicSize = newCamSize;

            // Move player to new area
            other.transform.position += movePlayer;

            camController.target = other.transform;
        }
    }

}
