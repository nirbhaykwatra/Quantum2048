using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PopUpSystem Class
 * -----------------
 * Separates the popup behavior into a dedicated script, independent from TileBoard.
 * This allows for easier management and access to all popup functionalities.
 */
public class PopUpSystem : MonoBehaviour
{
    // Reference to the tunneling popup GameObject.
    private GameObject tunnelingPopup;

    // Reference to the Info Button GameObject.
    private GameObject infoButton; 

    // Called when the script instance is being loaded.
    // Locates the Canvas and its child elements required for popup functionalities.
    private void Awake()
    {
        // Find the Canvas GameObject in the scene.
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Locate the "Tunnelling1" popup under the Canvas.
            tunnelingPopup = canvas.transform.Find("Tunnelling1")?.gameObject;

            // Locate the "Info Button" under the Canvas.
            infoButton = canvas.transform.Find("Info Button")?.gameObject;
        }
        else
        {
            Debug.LogWarning("Canvas GameObject not found in the scene!");
        }
    }

    // Activates the tunneling popup and initiates the pause behavior.
    public void Tunneling()
    {
        // Activate the tunneling popup.
        tunnelingPopup.SetActive(true);
        
        // Invoke the pause functionality.
        Pause();
    }

    // Handles the pause behavior related to popup interactions.
    // Currently, this method manages the visibility of the Info Button.
    public void Pause()
    {
        // If the tunneling popup is active, adjust UI accordingly.
        if (tunnelingPopup.activeSelf == true)
        {
            Debug.Log("popup");

            // Uncomment the following line if using timescale pausing:
            // Time.timeScale = 0;
            infoButton.SetActive(false);
        }
        else
        {
            Debug.Log("no popup");
           
            // Uncomment the following line if using timescale pausing:
            // Time.timeScale = 1;
            infoButton.SetActive(true);
        }
    }
}