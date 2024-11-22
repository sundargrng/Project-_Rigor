using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    public Transform platform;

    public Transform startPos;
    public Transform endPos;

    public float speed = 1.5f;

    int direction = 1;

    private bool isMoving = true; // Add a flag to control platform movement

    private void Update()
    {
        if (isMoving) // Check the flag before moving the platform
        {
            Vector2 target = currentMovementTarget();

            platform.position = Vector2.Lerp(platform.position, target, speed * Time.deltaTime);

            float distance = (target - (Vector2)platform.position).magnitude;

            if (distance <= 0.1f)
            {
                direction *= -1;
            }
        }
    }

    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return startPos.position;
        }
        else
        {
            return endPos.position;
        }
    }

    public void OnDrawGizmos()
    {
        if (platform != null && startPos != null && endPos != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPos.position);
            Gizmos.DrawLine(platform.transform.position, endPos.position);
        }
    }

    public void StopPlatform()
    {
        isMoving = false;
    }

    public void StartPlatform()
    {
        isMoving = true;
    }
}