using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    public Transform target;
    public float smoothing;

    public Vector2 minPosition;
    public Vector2 maxPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);

        if(transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    public void LoadData(GameData data)
    {
        // Load camera data
        transform.position = new Vector3(data.cameraPosition.x, data.cameraPosition.y, -10f);
        minPosition = data.cameraMinPosition;
        maxPosition = data.cameraMaxPosition;
        Camera.main.orthographicSize = data.cameraSize;
    }

    public void SaveData(ref GameData data)
    {
        // Save camera data
        data.cameraPosition = transform.position;
        data.cameraMinPosition = minPosition;
        data.cameraMaxPosition = maxPosition;
        data.cameraSize = Camera.main.orthographicSize;
    }
}
