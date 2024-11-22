using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    public GameObject bossObj;
    public GameObject bossHpUi;

    private void Start()
    {
        bossObj.SetActive(false);
        bossHpUi.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.PlayMusic(MusicType.ENEMIES_SPAWNNED);
            bossObj.SetActive(true);
            bossHpUi.SetActive(true);

            this.gameObject.SetActive(false);
        }
    }
}