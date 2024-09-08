using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script sperates the popup behavior into a seperate script from TileBoard so all popup behavior can be independent
// and easily accessed. 

public class PopUpSystem : MonoBehaviour
{
    // initializing popup gameobjects
    private GameObject tunnelingPopup;
    private GameObject infoButton; 
    

    private void Awake()
    {
        
        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            // Find Tunnelling1 under the Canvas
            tunnelingPopup = canvas.transform.Find("Tunnelling1")?.gameObject;

            // Find Info Button under the Canvas
            infoButton = canvas.transform.Find("Info Button")?.gameObject;
            
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
        // activate the info popup and pause. 
        tunnelingPopup.SetActive(true);
        
        Pause();
    }

       public void Pause()
    {
        // Checks to see if game is paused. will pause if unpaused, and unpause when paused. 

        // NOTE!!! time scale pauses animations. Have to find a work around for qubit to animate 
        if (Time.timeScale == 1)
        {
            Debug.Log("stopping time");

          // commenting out timescale pausing for now 
          //  Time.timeScale = 0;
            infoButton.SetActive(false);
        }
        else
        {
            Debug.Log("starting time");
           
           // Time.timeScale = 1;
            infoButton.SetActive(true);
        }
    }
}
