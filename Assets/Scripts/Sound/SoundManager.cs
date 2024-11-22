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
    LEVELUPSOUND,
    FINISH,
    GAME_OVER_SOUND,
    FOOTSTEP
}

public enum MusicType
{
    BACKGROUND_MUSIC_1,
    BACKGROUND_MUSIC_2,
    AREA_1,
    AREA_2,
    AREA_3,
    AREA_4,
    ENEMIES_SPAWNNED,
}

public enum AreaSound
{
    AREA_1,
    AREA_2,
    AREA_3,
    RAIN,
    FOREST,
    DUNGEON,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioClip[] musicList;
    [SerializeField] private AudioClip[] areaSoundList;

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

    public static void PlayMusic(MusicType musicType, float volume = 1)
    {
        if (instance == null || instance.audioSource == null)
        {
            Debug.LogError("SoundManager instance or AudioSource is null.");
            return;
        }

        int musicIndex = (int)musicType;
        if (musicIndex < instance.musicList.Length && instance.musicList[musicIndex] != null)
        {
            instance.audioSource.clip = instance.musicList[musicIndex];
            instance.audioSource.volume = volume;
            instance.audioSource.loop = true;
            instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning("MusicType " + musicType.ToString() + " is not assigned or is out of bounds.");
        }
    }

    public static void StopMusic()
    {
        if (instance != null && instance.audioSource != null && instance.audioSource.isPlaying)
        {
            instance.audioSource.Stop();
        }
    }

    public static void PlayAreaSound(AreaSound areaType, float volume = 1)
    {
        if (instance == null || instance.audioSource == null)
        {
            Debug.LogError("SoundManager instance or AudioSource is null.");
            return;
        }

        int areaIndex = (int)areaType;
        if (areaIndex < instance.areaSoundList.Length && instance.areaSoundList[areaIndex] != null)
        {
            instance.audioSource.clip = instance.areaSoundList[areaIndex];
            instance.audioSource.volume = volume;
            instance.audioSource.loop = true;
            instance.audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AreaType " + areaType.ToString() + " is not assigned or is out of bounds.");
        }
    }

    public static void StopAreaSound()
    {
        if (instance != null && instance.audioSource != null && instance.audioSource.isPlaying)
        {
            instance.audioSource.Stop();
        }
    }
}
