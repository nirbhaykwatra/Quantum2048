using UnityEngine;

public class ToggleMusicButton : MonoBehaviour
{
    // This function will be called when the button is clicked
    public void OnToggleMusicClicked()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ToggleMusic();
        }
    }
}
