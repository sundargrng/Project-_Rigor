using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransitions : MonoBehaviour
{

    private CameraController cam;

    public Vector2 newCamMinPos;
    public Vector2 newCamMaxPos;
    public Vector3 movePlayer;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            cam.minPosition = newCamMinPos;
            cam.maxPosition = newCamMaxPos;
            other.transform.position += movePlayer;
        }
    }
}
