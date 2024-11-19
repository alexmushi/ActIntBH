using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;

    // Singleton instance
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Set up singleton instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate AudioManagers
            return;
        }

        // Ensure this persists across scenes if needed
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // Method to get the current playback time of the music
    public float GetCurrentPlaybackTime()
    {
        return musicSource.time;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}

