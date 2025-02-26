using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
 * RadialProgress Class
 * ----------------------
 * Manages and updates two radial progress bars, handling game pausing and UI transitions when progress completes.
 * This script updates progress values, changes UI colors upon completion, and triggers subsequent UI elements and routines.
 */
public class RadialProgress : MonoBehaviour
{
    // Image for the first radial progress bar (original).
    [SerializeField] private Image image1;
    // Image for the second radial progress bar (new).
    [SerializeField] private Image image2;
    
    // Current value for the first progress bar (percentage out of 100).
    public float currentValue;
    // Current value for the second progress bar (percentage out of 100).
    public float currentValue2;
    
    // Audio source for playing notification sounds.
    public AudioSource src;
    // Notification sound clip.
    public AudioClip notification_sound_1;
    
    // Reference to the Icon Line GameObject for the first progress bar.
    private GameObject iconLine;
    // Reference to the Icon Line GameObject for the second progress bar.
    private GameObject iconLine2;
    
    // Reference to the Icon Dialog GameObject for the first progress bar.
    private GameObject iconDialog;
    // Reference to the Icon Dialog GameObject for the second progress bar.
    private GameObject iconDialog2;
    
    // Reference to the first radial progress GameObject.
    private GameObject radialProgress;
    // Reference to the second radial progress GameObject.
    private GameObject radialProgress2;
    
    // Reference to the Tunnelling1 GameObject.
    private GameObject tunnelling1;
    // Reference to the Tunnelling2 GameObject.
    private GameObject tunnelling2;
    
    // Reference to the Icon2 GameObject.
    private GameObject icon2;
    // Reference to the Icon Hat GameObject under Icon2.
    private GameObject iconHat;

    // Flag to indicate if the first progress is complete.
    private bool isProgressComplete = false;
    // Flag to indicate if the second progress is complete.
    private bool isProgressComplete2 = false;

    // Called before the first frame update.
    void Start()
    {
        // Find the Canvas GameObject.
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas == null)
        {
            Debug.LogError("Canvas GameObject not found.");
            return;
        }

        // Locate Tunnelling1 under the Canvas.
        Transform tunnelling1Transform = canvas.transform.Find("Tunnelling1");
        if (tunnelling1Transform == null)
        {
            Debug.LogError("Tunnelling1 GameObject not found under Canvas.");
            return;
        }
        tunnelling1 = tunnelling1Transform.gameObject;

        // Locate Tunnelling2 under the Canvas.
        Transform tunnelling2Transform = canvas.transform.Find("Tunnelling2");
        if (tunnelling2Transform == null)
        {
            Debug.LogError("Tunnelling2 GameObject not found under Canvas.");
            return;
        }
        tunnelling2 = tunnelling2Transform.gameObject;

        // Locate Competency Counter under the Canvas.
        Transform competencyCounter = canvas.transform.Find("Competency Counter");
        if (competencyCounter == null)
        {
            Debug.LogError("Competency Counter GameObject not found under Canvas.");
            return;
        }

        // Locate TunnellingProgress under Competency Counter.
        Transform tunnellingProgress = competencyCounter.Find("TunnellingProgress");
        if (tunnellingProgress == null)
        {
            Debug.LogError("TunnellingProgress GameObject not found under Competency Counter.");
            return;
        }

        // Locate Icon under TunnellingProgress.
        Transform icon = tunnellingProgress.Find("Icon");
        if (icon == null)
        {
            Debug.LogError("Icon GameObject not found under TunnellingProgress.");
            return;
        }

        // Locate Icon Line under Icon and deactivate it.
        Transform iconLineTransform = icon.Find("Icon Line");
        if (iconLineTransform != null)
        {
            iconLine = iconLineTransform.gameObject;
            iconLine.SetActive(false);
        }
        else
        {
            Debug.LogError("Icon Line GameObject not found under Icon.");
        }

        // Locate Icon Dialog under Icon and deactivate it.
        Transform iconDialogTransform = icon.Find("Icon Dialog");
        if (iconDialogTransform != null)
        {
            iconDialog = iconDialogTransform.gameObject;
            iconDialog.SetActive(false);
        }
        else
        {
            Debug.LogError("Icon Dialog GameObject not found under Icon.");
        }

        // Locate Icon2 under TunnellingProgress.
        Transform icon2Transform = tunnellingProgress.Find("Icon2");
        if (icon2Transform == null)
        {
            Debug.LogError("Icon2 GameObject not found under TunnellingProgress.");
            return;
        }
        // Store Icon2 reference and deactivate it.
        icon2 = icon2Transform.gameObject;
        icon2.SetActive(false);

        // Locate Icon Hat under Icon2 and deactivate it.
        Transform iconHatTransform = icon2Transform.Find("Icon Hat");
        if (iconHatTransform != null)
        {
            iconHat = iconHatTransform.gameObject;
            iconHat.SetActive(false);
        }
        else
        {
            Debug.LogError("Icon Hat GameObject not found under Icon2.");
        }

        // Locate Icon Line 2 under Icon2 and deactivate it.
        Transform iconLineTransform2 = icon2Transform.Find("Icon Line 2");
        if (iconLineTransform2 != null)
        {
            iconLine2 = iconLineTransform2.gameObject;
            iconLine2.SetActive(false);
        }
        else
        {
            Debug.LogError("Icon Line 2 GameObject not found under Icon2.");
        }

        // Locate Icon Dialog 2 under Icon2 and deactivate it.
        Transform iconDialogTransform2 = icon2Transform.Find("Icon Dialog 2");
        if (iconDialogTransform2 != null)
        {
            iconDialog2 = iconDialogTransform2.gameObject;
            iconDialog2.SetActive(false);
        }
        else
        {
            Debug.LogError("Icon Dialog 2 GameObject not found under Icon2.");
        }

        // Locate RadialProgress under TunnellingProgress and get its Image component.
        Transform radialProgressTransform = tunnellingProgress.Find("RadialProgress");
        if (radialProgressTransform != null)
        {
            radialProgress = radialProgressTransform.gameObject;
            Image tempImage = radialProgress.GetComponent<Image>();
            if (tempImage == null)
            {
                Debug.LogError("Image component not found on RadialProgress GameObject.");
                return;
            }
            image1 = tempImage;
        }
        else
        {
            Debug.LogError("RadialProgress GameObject not found under TunnellingProgress.");
        }

        // Locate RadialProgress2 under TunnellingProgress, get its Image component, and deactivate it initially.
        Transform radialProgressTransform2 = tunnellingProgress.Find("RadialProgress2");
        if (radialProgressTransform2 != null)
        {
            radialProgress2 = radialProgressTransform2.gameObject;
            Image tempImage2 = radialProgress2.GetComponent<Image>();
            if (tempImage2 == null)
            {
                Debug.LogError("Image component not found on RadialProgress2 GameObject.");
                return;
            }
            image2 = tempImage2;
            radialProgress2.SetActive(false); // Initially off until after stage 1 is done.
        }
        else
        {
            Debug.LogError("RadialProgress2 GameObject not found under TunnellingProgress.");
        }
    }

    // Called once per frame.
    void Update()
    {
        // Update the first radial progress bar if it's active and not yet complete.
        if (!isProgressComplete && image1 != null && radialProgress.activeSelf)
        {
            image1.fillAmount = currentValue / 100f;

            if (image1.fillAmount >= 1 && !isProgressComplete)
            {
                isProgressComplete = true;
                // Pause the game.
                Time.timeScale = 0;
                // Change the bar color.
                image1.color = new Color(208 / 255f, 71 / 255f, 195 / 255f);
                StartCoroutine(ProgressCompleteRoutine());
            }
        }

        // Update the second radial progress bar if it's active and not yet complete.
        if (!isProgressComplete2 && image2 != null && radialProgress2.activeSelf)
        {
            image2.fillAmount = currentValue2 / 100f;

            if (image2.fillAmount >= 1 && !isProgressComplete2)
            {
                isProgressComplete2 = true;
                // Pause the game.
                Time.timeScale = 0;
                // Change the bar color.
                image2.color = new Color(208 / 255f, 71 / 255f, 195 / 255f);
                StartCoroutine(ProgressCompleteRoutine2());
            }
        }
    }

    // Coroutine to handle the completion of the first progress bar.
    private IEnumerator ProgressCompleteRoutine()
    {
        // Wait for 1 second in real time.
        yield return new WaitForSecondsRealtime(1);

        // Set the game level to tunnelling2.
        GlobalData.level = "tunnelling2";

        // Play the notification sound.
        src.clip = notification_sound_1;
        src.Play();

        // Activate the first stage UI elements.
        if (iconLine != null) iconLine.SetActive(true);
        if (iconDialog != null) iconDialog.SetActive(true);

        // Hide the first radial progress bar.
        if (radialProgress != null) radialProgress.SetActive(false);

        // Deactivate tunnelling1 and activate tunnelling2.
        if (tunnelling1 != null) tunnelling1.SetActive(false);
        if (tunnelling2 != null) tunnelling2.SetActive(true);

        // Activate second stage UI elements.
        if (icon2 != null) icon2.SetActive(true);
        if (iconHat != null) iconHat.SetActive(true);
        if (radialProgress2 != null) radialProgress2.SetActive(true);
        
        // Unpause the game to progress to the second stage.
        Time.timeScale = 1;
    }

    // Coroutine to handle the completion of the second progress bar.
    private IEnumerator ProgressCompleteRoutine2()
    {
        // Wait for 1 second in real time.
        yield return new WaitForSecondsRealtime(1);

        // Activate the second set of UI elements.
        if (iconLine2 != null) iconLine2.SetActive(true);
        if (iconDialog2 != null) iconDialog2.SetActive(true);

        // Hide the second radial progress bar.
        if (radialProgress2 != null) radialProgress2.SetActive(false);

        // Unpause the game.
        Time.timeScale = 1;
    }
}