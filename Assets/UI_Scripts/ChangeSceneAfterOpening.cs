using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterOpening : MonoBehaviour
{
    [SerializeField]
    private float openingSceneTime;


    private void Update()
    {
        openingSceneTime -= Time.deltaTime;

        if (openingSceneTime < 0)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}
