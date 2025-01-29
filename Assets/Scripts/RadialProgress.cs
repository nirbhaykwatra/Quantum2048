using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RadialProgress : MonoBehaviour
{
    [SerializeField] private Image image1; // For RadialProgress (original)
    [SerializeField] private Image image2; // For RadialProgress2 (new)
    public float currentValue;
    public float currentValue2; // New variable for second progress bar
    public AudioSource src;
    public AudioClip notification_sound_1;
    private GameObject iconLine;
    private GameObject iconLine2;
    private GameObject iconDialog;
    private GameObject iconDialog2;
    private GameObject radialProgress;
    private GameObject radialProgress2;
    private GameObject tunnelling1;
    private GameObject tunnelling2;

    private GameObject icon2;    // Reference to Icon2 GameObject
    private GameObject iconHat;  // Reference to Icon Hat under Icon2

    private bool isProgressComplete = false; 
    private bool isProgressComplete2 = false; // New flag for second progress completion

    void Start()
    {
        // Find the Canvas first
        GameObject canvas = GameObject.Find("Canvas");

        if (canvas == null)
        {
            Debug.LogError("Canvas GameObject not found.");
            return;
        }

        Transform tunnelling1Transform = canvas.transform.Find("Tunnelling1");
        if (tunnelling1Transform == null)
        {
            Debug.LogError("Tunnelling1 GameObject not found under Canvas.");
            return;
        }
        tunnelling1 = tunnelling1Transform.gameObject;

        Transform tunnelling2Transform = canvas.transform.Find("Tunnelling2");
        if (tunnelling2Transform == null)
        {
            Debug.LogError("Tunnelling2 GameObject not found under Canvas.");
            return;
        }
        tunnelling2 = tunnelling2Transform.gameObject;

        Transform competencyCounter = canvas.transform.Find("Competency Counter");
        if (competencyCounter == null)
        {
            Debug.LogError("Competency Counter GameObject not found under Canvas.");
            return;
        }

        Transform tunnellingProgress = competencyCounter.Find("TunnellingProgress");
        if (tunnellingProgress == null)
        {
            Debug.LogError("TunnellingProgress GameObject not found under Competency Counter.");
            return;
        }

        Transform icon = tunnellingProgress.Find("Icon");
        if (icon == null)
        {
            Debug.LogError("Icon GameObject not found under TunnellingProgress.");
            return;
        }

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

        Transform icon2Transform = tunnellingProgress.Find("Icon2");
        if (icon2Transform == null)
        {
            Debug.LogError("Icon2 GameObject not found under TunnellingProgress.");
            return;
        }

        // Store icon2 reference
        icon2 = icon2Transform.gameObject;
        icon2.SetActive(false);

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
            radialProgress2.SetActive(false); // Initially off until after stage 1 is done
        }
        else
        {
            Debug.LogError("RadialProgress2 GameObject not found under TunnellingProgress.");
        }
    }

    void Update()
    {
        // Update first radial bar if not complete
        if (!isProgressComplete && image1 != null && radialProgress.activeSelf)
        {
            image1.fillAmount = currentValue / 100f;

            if (image1.fillAmount >= 1 && !isProgressComplete)
            {
                isProgressComplete = true;
                // Pause the game
                Time.timeScale = 0;
                // Re-colour the bar
                image1.color = new Color(208 / 255f, 71 / 255f, 195 / 255f);
                StartCoroutine(ProgressCompleteRoutine());
            }
        }

        // Update second radial bar if active and not complete
        if (!isProgressComplete2 && image2 != null && radialProgress2.activeSelf)
        {
            image2.fillAmount = currentValue2 / 100f;

            if (image2.fillAmount >= 1 && !isProgressComplete2)
            {
                isProgressComplete2 = true;
                // Pause the game
                Time.timeScale = 0;
                // Re-colour the bar
                image2.color = new Color(208 / 255f, 71 / 255f, 195 / 255f);
                StartCoroutine(ProgressCompleteRoutine2());
            }
        }
    }

    private IEnumerator ProgressCompleteRoutine()
    {
        // Wait for 1 second in real time
        yield return new WaitForSecondsRealtime(1);

        GlobalData.level = "tunnelling2"; // Set the level to tunnelling2

        src.clip = notification_sound_1;
        src.Play();

        // Show Icon Line and Icon Dialog
        if (iconLine != null) iconLine.SetActive(true);
        if (iconDialog != null) iconDialog.SetActive(true);

        // Hide the first radialProgress
        if (radialProgress != null) radialProgress.SetActive(false);

        // Deactivate tunnelling1 and activate tunnelling2
        if (tunnelling1 != null) tunnelling1.SetActive(false);
        if (tunnelling2 != null) tunnelling2.SetActive(true);

        // Activate second stage UI elements
        if (icon2 != null) icon2.SetActive(true);
        if (iconHat != null) iconHat.SetActive(true);
        if (radialProgress2 != null) radialProgress2.SetActive(true);
        // Unpause the game so we can progress to the second stage naturally
        Time.timeScale = 1;
    }

    private IEnumerator ProgressCompleteRoutine2()
    {
        // Wait for 1 second in real time
        yield return new WaitForSecondsRealtime(1);

        // Show second line and dialog
        if (iconLine2 != null) iconLine2.SetActive(true);
        if (iconDialog2 != null) iconDialog2.SetActive(true);

        // Hide the second radialProgress
        if (radialProgress2 != null) radialProgress2.SetActive(false);

        // Unpause the game
        Time.timeScale = 1;
    }
}