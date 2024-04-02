using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition2 : MonoBehaviour
{
    private Camera cam;
    private CameraController camController;
    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public Vector3 movePlayer;

    public float newCamSize;

    //private bool inTransition = false;

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
            //inTransition = true;

            // Perform camera transition
            camController.minPosition = newCamMinPos;
            camController.maxPosition = newCamMaxPos;

            cam.orthographicSize = newCamSize;

            other.transform.position += movePlayer;

            camController.target = other.transform;
        }
    }
}
