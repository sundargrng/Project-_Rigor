using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SWORD,
    FLYINGSLASH,
    ARROWHURT,
    INSTANTIATEARROW,
    FIREHURT,
    INSTANTIATEFIREBALL,
    DRAGONDEF,
    STONEHURT,
    INSTANTIATESTONE,
    ENEMYDEATH,
    SLASHDAMAGE,
    SWORDDAMAGE,
    FOOTSTEP
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundList;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SoundManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // If another SoundManager instance exists, destroy this one
            return;
        }

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on SoundManager gameObject.");
        }
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        if (instance == null || instance.audioSource == null)
        {
            Debug.LogError("SoundManager instance or AudioSource is null.");
            return;
        }

        if ((int)sound < instance.soundList.Length && instance.soundList[(int)sound] != null)
        {
            instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
        }
        else
        {
            Debug.LogWarning("SoundType " + sound.ToString() + " is not assigned or is out of bounds.");
        }
    }
}
