using UnityEngine;

/*
 * AudioManager Class
 * ------------------
 * Manages background music playback using the singleton pattern, ensuring that only one instance persists across scenes.
 */
public class AudioManager : MonoBehaviour
{
    // Singleton instance of the AudioManager.
    public static AudioManager Instance;

    // AudioSource component used to play and control background music.
    private AudioSource audioSource;

    // Called when the script instance is being loaded.
    // Implements the singleton pattern, sets up the AudioSource, and begins music playback.
    private void Awake()
    {
        // Ensure that only one instance of AudioManager exists across scenes.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist this GameObject through scene changes.
        }
        else
        {
            // Destroy any duplicate AudioManager instances.
            Destroy(gameObject);
            return;
        }

        // Get the AudioSource component attached to this GameObject.
        audioSource = GetComponent<AudioSource>();

        // Configure the AudioSource to loop the music.
        audioSource.loop = true;

        // Start playing the music if it is not already playing.
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Toggles the background music playback.
    // Pauses the music if it's currently playing, or resumes playback if it is paused.
    public void ToggleMusic()
    {
        if (audioSource.isPlaying)
        {
            // Pause the music, retaining the current playback position.
            audioSource.Pause();
        }
        else
        {
            // Resume playback from the paused position.
            // Note: If the AudioSource has never played, UnPause() behaves like Play().
            audioSource.UnPause();
        }
    }
}
