using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist through scene changes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate if it exists
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (!audioSource.isPlaying) audioSource.Play(); // Start playing by default
    }

    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();  // Pauses at the current playback position
        }
        else
        {
            // If the AudioSource is paused, UnPause() will resume from the last position
            // If the AudioSource has never played, UnPause() acts like Play()
            audioSource.UnPause(); 
        }
    }

}
