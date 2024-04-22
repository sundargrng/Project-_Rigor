using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicController : MonoBehaviour
{
    [SerializeField] private AreaSound sound;
    [SerializeField] private bool playOnEnter = true;
    [SerializeField] private bool stopOnExit = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playOnEnter)
            {
                SoundManager.PlayAreaSound(sound);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (stopOnExit)
            {
                SoundManager.StopMusic();
            }
        }
    }
}