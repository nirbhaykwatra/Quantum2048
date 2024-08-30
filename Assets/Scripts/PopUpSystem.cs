using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script sperates the popup behavior into a seperate script from TileBoard so all popup behavior can be independent in the future.

public class PopUpSystem : MonoBehaviour
{

    private GameObject tunnelingPopup;
    private GameObject infoButton; 
    

    private void Awake()
    {

        //Tunneling();
        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Find Tunnelling1 under the Canvas
            tunnelingPopup = canvas.transform.Find("Tunnelling1")?.gameObject;

            // Find Info Button under the Canvas
            infoButton = canvas.transform.Find("Info Button")?.gameObject;

            if (tunnelingPopup != null && infoButton != null)
            {
                tunnelingPopup.SetActive(false); // Ensure it starts inactive
                infoButton.SetActive(false); // Ensure it starts inactive
            }
            else
            {
                Debug.LogWarning("Either Tunnelling1 or Info Button GameObjects were not found under Canvas!");
            }
        }
        else
        {
            Debug.LogWarning("Canvas GameObject not found in the scene!");
        }
    }


       public void Tunneling()
    {
        // debug to check that script has been successfully accessed. 
        Debug.Log("popup script accessed");
        
        tunnelingPopup.SetActive(true);
        
        Pause();
    }

       public void Pause()
    {
        if (Time.timeScale == 1)
        {
            Debug.Log("stopping time");
            Time.timeScale = 0;
            infoButton.SetActive(false);
        }
        else
        {
            Debug.Log("starting time");
            Time.timeScale = 1;
            infoButton.SetActive(true);
        }
    }
}
